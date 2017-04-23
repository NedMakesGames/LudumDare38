using Baluga3.GameFlowLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.GameLogic.PlayerStates {
    public class JumpHoldTimer : PlayerMoveManager.StateController {

        private float startJumpVel;
        private float timer;

        public JumpHoldTimer(PlayerMoveManager manager) : base(manager) {
            
        }

        public override void Enter() {
            player.body.gravity = false;
            startJumpVel = player.body.vel.y;
            timer = 0;
        }

        public override void Tick(float deltaTime) {
            timer += deltaTime;
            player.noGravTimer -= deltaTime;
            manager.DoHorizAccel(manager.HorizInput() * pconsts.player.maxVel, pconsts.player.jumpHorizAccel * deltaTime);
            if(manager.CheckJumpHoldEnd(startJumpVel, timer)) {
                manager.TransferState(PlayerMoveManager.State.Jump);
            }
        }
    }
}
