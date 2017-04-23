using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld.GameLogic.PlayerStates {
    class Jump : PlayerMoveManager.StateController {

        private enum AirJumpState {
            Ready, Pound, Timer
        }

        private float airJumpTimer;
        private AirJumpState airJumpState;

        public Jump(PlayerMoveManager manager) : base(manager) {

        }

        public override void Enter() {
            airJumpTimer = 0;
            airJumpState = AirJumpState.Ready;
        }

        public override void Tick(float deltaTime) {
            player.noGravTimer -= deltaTime;
            player.body.gravity = player.noGravTimer <= 0;
            manager.DoHorizAccel(manager.HorizInput() * pconsts.player.maxVel, pconsts.player.jumpHorizAccel * deltaTime);
            if(player.body.grounded) {
                player.multiJumpTimer = 0;
                manager.TransferState(PlayerMoveManager.State.Grounded);
            } else if(player.flipping == PlayerCharacter.FlipMode.Flipping && !manager.IsCrouchPressed()) {
                player.flipping = PlayerCharacter.FlipMode.Released;
            } else if(player.flipping == PlayerCharacter.FlipMode.None) {
                switch(airJumpState) {
                case AirJumpState.Ready:
                    if(manager.IsCrouchPressed()) {
                        player.body.vel.x *= pconsts.player.airJumpDrag;
                        player.body.vel.y = 0;
                        airJumpTimer = 0;
                        airJumpState = AirJumpState.Pound;
                    }
                    break;
                case AirJumpState.Pound:
                    airJumpTimer += deltaTime;
                    if(airJumpTimer >= pconsts.player.airPoundLeeway) {
                        manager.DoGroundPound();
                        manager.TransferState(PlayerMoveManager.State.GroundPound);
                    } else if(manager.HasJumpTrigger()) {
                        manager.DoAirJump();
                        airJumpTimer = 0;
                        airJumpState = AirJumpState.Timer;
                    }
                    break;
                case AirJumpState.Timer:
                    airJumpTimer += deltaTime;
                    if(airJumpTimer >= pconsts.player.airJumpCooldown) {
                        airJumpState = AirJumpState.Ready;
                    }
                    break;
                }
            }
        }
    }
}
