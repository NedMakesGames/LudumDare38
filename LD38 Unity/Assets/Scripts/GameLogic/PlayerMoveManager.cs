using Baluga3.GameFlowLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.GameLogic {
    public class PlayerMoveManager : ITicking {

        public abstract class StateController {

            protected PlayerMoveManager manager;
            protected PhysicsConstants pconsts;
            protected PlayerCharacter player;
            protected AudioRegistry audio;

            public StateController(PlayerMoveManager manager) {
                this.manager = manager;
                this.player = manager.player;
                this.pconsts = manager.pconsts;
                this.audio = manager.audio;
            }

            public abstract void Enter();
            public abstract void Tick(float deltaTime);
        }

        // Normal jump: for gaining height
        // Backflip: Does not ramp up in height, useful for racking up multijump counters
        // Long jump: For avoiding comets - stay low to the ground
        // Crouch in the air: quickly move stright down
        // Crouch then jump quickly: freeze momentum, and execute one small jump
        // Switch jump: quickly move horizontally in the opposite direction while landing and then jump to gain extra height
        // Spin jump: quickly move horizontally back and forth while landing and then jump to rocket straight upwards


        public enum State {
            Grounded,
            JumpHold,
            Jump,
            Crouch,
            Sliding,
            GroundPound,
            GameOver,
            Count,
        }

        private bool playerUnregistered;
        private PlayerCharacter player;
        private Command<PhysicsBody> regCmd;
        private SubscribableFloat angleAxis;
        private SubscribableBool jumpBtn;
        private SubscribableBool crouchBtn;
        private GameState gameState;
        private AudioRegistry audio;

        private PhysicsConstants pconsts;
        private State activeState;
        private List<StateController> states;

        private float jumpLeeway;
        private bool lastJumpBtn;
        private float jumpFlairAnimTimer;

        public PlayerMoveManager(AutoController ctrlr) {
            player = ctrlr.Components.GetOrRegister<PlayerCharacter>((int)ComponentKeys.PlayerCharacter, PlayerCharacter.Create);
            regCmd = ctrlr.Components.GetOrRegister<Command<PhysicsBody>>((int)ComponentKeys.AddPhysicsBody, Command<PhysicsBody>.Create);
            angleAxis = ctrlr.Components.GetOrRegister<SubscribableFloat>((int)ComponentKeys.PlayerAngleAxis, SubscribableFloat.Create);
            jumpBtn = ctrlr.Components.GetOrRegister<SubscribableBool>((int)ComponentKeys.PlayerJumpBtn, SubscribableBool.Create);
            crouchBtn = ctrlr.Components.GetOrRegister<SubscribableBool>((int)ComponentKeys.PlayerCrouchBtn, SubscribableBool.Create);
            pconsts = ctrlr.Game.Components.GetOrRegister<PhysicsConstants>((int)ComponentKeys.PhysicsConstants, PhysicsConstants.Create);
            gameState = ctrlr.Components.GetOrRegister<GameState>((int)ComponentKeys.GameState, GameState.Create);
            audio = ctrlr.Components.GetOrRegister<AudioRegistry>((int)ComponentKeys.AudioRegistry, AudioRegistry.Create);
            ctrlr.Components.GetOrRegister<Message<Vector2>>((int)ComponentKeys.OnBouncyCollision, Message<Vector2>.Create)
                .Subscribe(new SimpleListener<Vector2>(OnBouncyCollision));

            player.body.height = pconsts.player.height;
            player.body.gravity = true;
            player.facing = 1;

            states = new List<StateController>();
            for(int i = 0; i < (int)State.Count; i++) {
                states.Add(null);
            }
            states[(int)State.Grounded] = new PlayerStates.Grounded(this);
            states[(int)State.JumpHold] = new PlayerStates.JumpHoldTimer(this);
            states[(int)State.Jump] = new PlayerStates.Jump(this);
            states[(int)State.Crouch] = new PlayerStates.CrouchGrounded(this);
            states[(int)State.Sliding] = new PlayerStates.Sliding(this);
            states[(int)State.GroundPound] = new PlayerStates.GroundPound(this);
            states[(int)State.GameOver] = new PlayerStates.GameOver(this);
            TransferState(State.Grounded);

            playerUnregistered = true;
            ctrlr.ActivatableList.Add(new TickingJanitor(ctrlr.Game, this));

            gameState.phase.EnterStateMessenger.Subscribe(new SimpleListener<int>((s) => OnGamePhaseChange()));
        }

        private void OnGamePhaseChange() {
            if(gameState.phase.State == (int)GamePhase.GameOver) {
                TransferState(State.GameOver);
            }
        }

        public int GetTickPriority() {
            return (int)TickPriority.Normal;
        }

        public void TransferState(State state) {
            activeState = state;
            //Debug.Log(string.Format("{0}: New state {1}", Time.frameCount, state));
            states[(int)activeState].Enter();
        }

        public float HorizInput() {
            return angleAxis.Value;
        }

        public void DoHorizAccel(float target, float change) {
            player.body.vel.x = Mathf.MoveTowards(player.body.vel.x, target, change);
        }

        public bool HasMoveInput() {
            return Mathf.Abs(HorizInput()) >= pconsts.player.axisSnapThreshhold;
        }

        public bool HasJumpTrigger() {
            return jumpLeeway > 0;
        }

        public bool IsJumpPressed() {
            return jumpBtn.Value;
        }

        public bool IsCrouchPressed() {
            return crouchBtn.Value;
        }

        private void DoJump(float vel) {
            jumpLeeway = 0;
            player.body.vel.y = vel;
            player.multiJumpTimer = 0;
        }

        public float MultiJumpMultiplier(float scaling) {
            //return 1 + player.multiJumpCount * scaling;
            return 1 + Mathf.Pow(player.multiJumpCount, scaling);
        }

        public bool RefreshMultiJumpTimer(float deltaTime) {
            player.multiJumpTimer += deltaTime;
            if(player.multiJumpTimer >= pconsts.player.landingMultiJumpPeriod) {
                ClearMultiJump();
                return false;
            } else {
                return true;
            }
        }

        public void ClearMultiJump() {
            //Debug.Log(string.Format("{0}: Reset multi jump", Time.frameCount));
            player.multiJumpTimer = pconsts.player.landingMultiJumpPeriod;
            player.multiJumpCount = 0;
            player.flipping = PlayerCharacter.FlipMode.None;
        }

        public void DoHighJump() {
            //Debug.Log(string.Format("{0}: High jump", Time.frameCount));
            DoJump(pconsts.player.jumpVel * MultiJumpMultiplier(pconsts.player.jumpHeightScaling));
            audio.highJump.Play();
            player.flipping = PlayerCharacter.FlipMode.None;
        }

        public void DoSwitchJump() {
            //Debug.Log(string.Format("{0}: Switch jump", Time.frameCount));
            player.body.vel.x = -player.body.vel.x * pconsts.player.switchJumpMoveMult;
            DoJump((pconsts.player.switchJumpVel + Mathf.Abs(player.body.vel.x) * pconsts.player.switchJumpHeightHorizScale)
                * MultiJumpMultiplier(pconsts.player.jumpHeightScaling));
            audio.switchJump.Play();
            player.flipping = PlayerCharacter.FlipMode.None;
        }

        public void DoSpinJump() {
            //Debug.Log(string.Format("{0}: Spin jump", Time.frameCount));
            DoJump(pconsts.player.spinJumpVel * MultiJumpMultiplier(pconsts.player.jumpHeightScaling));
            player.body.vel.x = 0;
            audio.spinJump.Play();
            player.flipping = PlayerCharacter.FlipMode.None;
        }

        public void DoLowJump() {
            //Debug.Log(string.Format("{0}: Low jump", Time.frameCount));
            DoJump(pconsts.player.lowJumpVel * MultiJumpMultiplier(pconsts.player.jumpHeightScaling));
            player.body.vel.x = player.facing * pconsts.player.lowJumpHorizVel * MultiJumpMultiplier(pconsts.player.backflipHorizScaling);
            //player.noGravTimer = 0;
            audio.lowJump.Play();
            player.flipping = PlayerCharacter.FlipMode.None;
        }

        public void DoBackflip() {
            //Debug.Log(string.Format("{0}: Back jump {1}", Time.frameCount, player.multiJumpCount));
            DoJump(pconsts.player.backflipJumpVel * MultiJumpMultiplier(pconsts.player.jumpHeightScaling));
            player.flipping = PlayerCharacter.FlipMode.Flipping;
            player.body.vel.x = -player.facing * pconsts.player.backflipHorizVel * MultiJumpMultiplier(pconsts.player.backflipHorizScaling);
            audio.backJump.Play();
        }

        public void DoBackflipLaunchJump() {
            //Debug.Log(string.Format("{0}: Back launch jump {1}", Time.frameCount, player.multiJumpCount));
            player.flipping = PlayerCharacter.FlipMode.None;
            DoJump(pconsts.player.backflipLaunchJumpVel * MultiJumpMultiplier(pconsts.player.jumpHeightScaling));
            player.body.vel.x = player.facing * pconsts.player.backflipHorizVel * MultiJumpMultiplier(pconsts.player.backflipHorizScaling);
            audio.highJump.Play();
        }

        public void DoGroundPound() {
            player.body.vel.x = 0;
            player.body.vel.y = -pconsts.player.groundPoundSpeed;
            player.flipping = PlayerCharacter.FlipMode.None;
        }

        public void DoAirJump() {
            player.body.vel.y = pconsts.player.airJumpSpeed;
            audio.airJump.Play();
            player.flipping = PlayerCharacter.FlipMode.None;
        }

        private void OnBouncyCollision(Vector2 pos) {
            player.hitBounce = true;
            if(player.flipping == PlayerCharacter.FlipMode.Flipping) {
                DoBackflip();
            } else {
                player.flipping = PlayerCharacter.FlipMode.None;
                DoHighJump();
            }
            TransferState(State.JumpHold);
        }

        public void UpdateFacing() {
            if(player.body.grounded) {
                float m = HorizInput();
                if(m > 0) {
                    player.facing = 1;
                } else if(m < 0) {
                    player.facing = -1;
                }
            }
        }

        public bool CheckJumpHoldEnd(float startSpeed, float timer) {
            if((!IsJumpPressed() && !player.hitBounce) || timer >= pconsts.player.jumpHoldTime) {
                player.hitBounce = false;
                float holdPerc = Mathf.Clamp01(timer / pconsts.player.jumpHoldTime);
                player.multiJumpCount += holdPerc;
                player.body.vel.y *= Mathf.Lerp(pconsts.player.jumpHoldSpeedMinMult, 1, holdPerc);
                return true;
            } else {
                player.body.vel.y = startSpeed;
                return false;
            }
        }

        public void Tick(float deltaTime) {
            if(playerUnregistered) {
                playerUnregistered = false;
                regCmd.Send(player.body);
            }

            jumpLeeway -= deltaTime;
            if(jumpBtn.Value && !lastJumpBtn) {
                jumpLeeway = pconsts.player.jumpTriggerLeeway;
            }
            lastJumpBtn = jumpBtn.Value;

            states[(int)activeState].Tick(deltaTime);

            if(activeState == State.Jump || activeState == State.JumpHold) {
                jumpFlairAnimTimer -= deltaTime;
                if(jumpFlairAnimTimer <= 0) {
                    if(player.body.vel.y <= 0) {
                        player.anim = PlayerCharacter.Animation.Fall;
                    } else {
                        player.anim = PlayerCharacter.Animation.Jump;
                    }
                }
            }
        }
    }
}
