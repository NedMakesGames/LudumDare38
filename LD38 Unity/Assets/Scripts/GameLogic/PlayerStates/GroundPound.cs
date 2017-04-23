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
            player.body.gravity = true;
        }

        public override void Tick(float deltaTime) {
            manager.DoHorizAccel(manager.HorizInput() * pconsts.player.maxVel, pconsts.player.groundPoundHorizAccel * deltaTime);
            if(player.body.grounded) {
                player.multiJumpTimer = 0;
                manager.TransferState(PlayerMoveManager.State.Grounded);
            }
        }
    }
}
