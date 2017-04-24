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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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
            player.body.height = pconsts.player.height;
        }

        public override void Tick(float deltaTime) {
            //player.noGravTimer -= deltaTime;
            //player.body.gravity = player.noGravTimer <= 0;
            manager.DoHorizAccel(manager.HorizInput() * pconsts.player.maxVel, pconsts.player.jumpHorizAccel * deltaTime);
            if(player.body.grounded) {
                audio.land.Play();
                manager.TransferState(PlayerMoveManager.State.Grounded);
            } else if(player.flipping == PlayerCharacter.FlipMode.Flipping && !manager.IsCrouchPressed()) {
                player.flipping = PlayerCharacter.FlipMode.Released;
            } else if(player.flipping == PlayerCharacter.FlipMode.None) {
                switch(airJumpState) {
                case AirJumpState.Ready:
                    if(manager.IsCrouchPressed()) {
                        player.body.vel.x *= pconsts.player.airJumpDrag;
                        player.body.vel.y = Mathf.Min(0, player.body.vel.y);
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
