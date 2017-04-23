using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.GameLogic.PlayerStates {
    public class Sliding : PlayerMoveManager.StateController {

        public Sliding(PlayerMoveManager manager) : base(manager) {

        }

        public override void Enter() {

        }

        public override void Tick(float deltaTime) {
            manager.RefreshMultiJumpTimer(deltaTime);
            manager.DoHorizAccel(manager.HorizInput() * pconsts.player.crouchMaxVel, pconsts.player.slideAccel * deltaTime);
            if(!manager.IsCrouchPressed()) {
                manager.TransferState(PlayerMoveManager.State.Grounded);
            } else if(Mathf.Abs(player.body.vel.x) <= pconsts.player.crouchMaxVel) {
                manager.TransferState(PlayerMoveManager.State.Crouch);
            } else if(manager.HasJumpTrigger()) {
                manager.DoLowJump();
                manager.TransferState(PlayerMoveManager.State.Jump);
            }
        }
    }
}
