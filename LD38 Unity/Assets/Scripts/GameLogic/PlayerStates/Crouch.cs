using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld.GameLogic.PlayerStates {
    public class CrouchGrounded : PlayerMoveManager.StateController {
        public CrouchGrounded(PlayerMoveManager manager) : base(manager) {

        }

        public override void Enter() {
            //player.body.gravity = true;
            player.body.height = pconsts.player.crouchHeight;
            player.anim = PlayerCharacter.Animation.Crouch;
            audio.crouch.Play();
        }

        public override void Tick(float deltaTime) {
            manager.UpdateFacing();
            manager.DoHorizAccel(manager.HorizInput() * pconsts.player.crouchMaxVel, pconsts.player.crouchAccel * deltaTime);
            if(!manager.IsCrouchPressed()) {
                manager.TransferState(PlayerMoveManager.State.Grounded);
            } else if(manager.HasJumpTrigger()) {
                manager.DoBackflip();
                manager.TransferState(PlayerMoveManager.State.JumpHold);
            }
        }
    }
}
