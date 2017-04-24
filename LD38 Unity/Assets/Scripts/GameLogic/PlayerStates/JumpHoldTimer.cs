// Copyright (c) 2017, Timothy Ned Atton.
// All rights reserved.
// nedmakesgames@gmail.com
// This code was written while streaming on twitch.tv/nedmakesgames
//
// This file is part of my Ludum Dare 38 Entry.
//
// My Ludum Dare 38 Entry is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// My Ludum Dare 38 Entry is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with My Ludum Dare 38 Entry.  If not, see <http://www.gnu.org/licenses/>.

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
            //player.body.gravity = false;
            startJumpVel = player.body.vel.y;
            timer = 0;
            player.body.height = pconsts.player.height;
        }

        public override void Tick(float deltaTime) {
            timer += deltaTime;
            //player.noGravTimer -= deltaTime;
            manager.DoHorizAccel(manager.HorizInput() * pconsts.player.maxVel, pconsts.player.jumpHorizAccel * deltaTime);
            if(manager.CheckJumpHoldEnd(startJumpVel, timer)) {
                manager.TransferState(PlayerMoveManager.State.Jump);
            }
        }
    }
}
