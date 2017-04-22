using Baluga3.GameFlowLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.GameLogic {
    public class PlayerMoveManager : ITicking {

        public enum JumpState {
            None, Ready, Holding,
        }

        private bool playerUnregistered;
        private PlayerCharacter player;
        private Command<PhysicsBody> regCmd;
        private SubscribableFloat angleAxis;
        private SubscribableBool jumpBtn;
        private PhysicsConstants pconsts;

        private JumpState jumpState;
        private bool playerWasMoving;
        private bool lastJumpBtn;
        private float jumpTimer;

        public PlayerMoveManager(AutoController ctrlr) {
            player = ctrlr.Components.GetOrRegister<PlayerCharacter>((int)ComponentKeys.PlayerCharacter, PlayerCharacter.Create);
            regCmd = ctrlr.Components.GetOrRegister<Command<PhysicsBody>>((int)ComponentKeys.AddPhysicsBody, Command<PhysicsBody>.Create);
            angleAxis = ctrlr.Components.GetOrRegister<SubscribableFloat>((int)ComponentKeys.PlayerAngleAxis, SubscribableFloat.Create);
            jumpBtn = ctrlr.Components.GetOrRegister<SubscribableBool>((int)ComponentKeys.PlayerJumpBtn, SubscribableBool.Create);
            pconsts = ctrlr.Game.Components.GetOrRegister<PhysicsConstants>((int)ComponentKeys.PhysicsConstants, PhysicsConstants.Create);

            player.body.height = pconsts.player.height;

            jumpState = JumpState.Ready;
            playerUnregistered = true;
            ctrlr.ActivatableList.Add(new TickingJanitor(ctrlr.Game, this));
        }

        public int GetTickPriority() {
            return (int)TickPriority.Normal;
        }

        public void Tick(float deltaTime) {
            if(playerUnregistered) {
                playerUnregistered = false;
                regCmd.Send(player.body);
            }

            bool moving = Mathf.Abs(angleAxis.Value) >= pconsts.player.axisSnapThreshhold;
            if(moving && !playerWasMoving) {
                player.body.vel.x = angleAxis.Value * pconsts.player.snapVel;
            }
            playerWasMoving = moving;

            if(moving) {
                player.body.vel.x = Mathf.MoveTowards(
                    player.body.vel.x, 
                    Mathf.Sign(angleAxis.Value) * pconsts.player.maxVel, 
                    pconsts.player.upAccel * deltaTime);
            } else {
                player.body.vel.x = Mathf.MoveTowards(
                    player.body.vel.x, 
                    0, 
                    pconsts.player.downAccel * deltaTime);
            }

            switch(jumpState) {
            case JumpState.Ready:
                if(player.body.grounded && jumpBtn.Value && !lastJumpBtn) {
                    player.body.vel.y = pconsts.player.jumpVel;
                    jumpTimer = 0;
                    jumpState = JumpState.Holding;
                }
                break;
            case JumpState.Holding:
                jumpTimer += deltaTime;
                if(!jumpBtn.Value || jumpTimer >= pconsts.player.jumpHoldTime) {
                    player.body.vel.y = Mathf.Lerp(pconsts.player.jumpHoldSpeedMinMult, 1, jumpTimer / pconsts.player.jumpHoldTime);
                    jumpState = JumpState.Ready;
                }
                break;
            }
            lastJumpBtn = jumpBtn.Value;
        }
    }
}
