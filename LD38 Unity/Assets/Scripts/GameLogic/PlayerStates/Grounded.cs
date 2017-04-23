using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.GameLogic.PlayerStates {
    public class Grounded : PlayerMoveManager.StateController {

        private bool lastMoveInput;
        private float spinClearTimer;

        public Grounded(PlayerMoveManager manager) : base(manager) {

        }

        public override void Enter() {
            player.body.gravity = false;
            lastMoveInput = manager.HasMoveInput();
            player.noGravTimer = 0;
            player.spinCount = 0;
        }

        public override void Tick(float deltaTime) {
            int facing = player.facing;
            manager.UpdateFacing();
            if(facing != player.facing) {
                spinClearTimer = 0;
                player.spinCount++;
            }

            spinClearTimer += deltaTime;
            if(spinClearTimer >= pconsts.player.spinJumpPeriod) {
                player.spinCount = 0;
            }

            manager.RefreshMultiJumpTimer(deltaTime);
            if(manager.HasMoveInput()) {
                if(!lastMoveInput) {
                    player.body.vel.x = manager.HorizInput() * pconsts.player.snapVel;
                }
                lastMoveInput = true;
                manager.DoHorizAccel(manager.HorizInput() * pconsts.player.maxVel, pconsts.player.upAccel * deltaTime);
            } else {
                lastMoveInput = false;
                manager.DoHorizAccel(0, pconsts.player.downAccel * deltaTime);
            }

            if(player.flipping == PlayerCharacter.FlipMode.Flipping && !manager.IsCrouchPressed()) {
                manager.ClearMultiJump();
            }
            if(manager.HasJumpTrigger()) {
                if(player.flipping == PlayerCharacter.FlipMode.Released) {
                    manager.DoBackflipLaunchJump();
                } else if(player.spinCount > 2) {
                    manager.DoSpinJump();
                } else { 
                    int velocitySign = (int)Mathf.Sign(player.body.vel.x);
                    if(manager.HasMoveInput() && player.facing != velocitySign) {
                        manager.DoSwitchJump();
                    } else {
                        manager.DoHighJump();
                    }
                }
                manager.TransferState(PlayerMoveManager.State.JumpHold);
            } else if(manager.IsCrouchPressed()) {
                player.spinCount = 0;
                if(lastMoveInput) {
                    manager.TransferState(PlayerMoveManager.State.Sliding);
                } else {
                    manager.TransferState(PlayerMoveManager.State.Crouch);
                }
            }
        }
    }
}
