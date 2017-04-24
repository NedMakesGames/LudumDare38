using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld.GameLogic.PlayerStates {
    public class GroundPound : PlayerMoveManager.StateController {

        public GroundPound(PlayerMoveManager manager) : base(manager) {

        }

        public override void Enter() {
            player.multiJumpCount = 0;
            //player.body.gravity = true;
            player.body.height = pconsts.player.height;
            audio.groundPound.Play();
        }

        public override void Tick(float deltaTime) {
            manager.DoHorizAccel(manager.HorizInput() * pconsts.player.maxVel, pconsts.player.groundPoundHorizAccel * deltaTime);
            if(player.body.grounded) {
                player.multiJumpCount = 0;
                player.multiJumpTimer = pconsts.player.landingMultiJumpPeriod;
                audio.land.Play();
                manager.TransferState(PlayerMoveManager.State.Grounded);
            }
        }
    }
}
