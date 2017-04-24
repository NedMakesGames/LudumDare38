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
    public class Grounded : PlayerMoveManager.StateController {

        private bool lastMoveInput;
        private float spinClearTimer;

        public Grounded(PlayerMoveManager manager) : base(manager) {

        }

        public override void Enter() {
            //player.body.gravity = false;
            lastMoveInput = manager.HasMoveInput();
            //player.noGravTimer = 0;
            player.spinCount = 0;
            player.body.height = pconsts.player.height;
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
                    audio.startMove.Play();
                    player.body.vel.x = manager.HorizInput() * pconsts.player.snapVel;
                }
                lastMoveInput = true;
                manager.DoHorizAccel(manager.HorizInput() * pconsts.player.maxVel, pconsts.player.upAccel * deltaTime);
                player.anim = PlayerCharacter.Animation.Walk;
            } else {
                lastMoveInput = false;
                manager.DoHorizAccel(0, pconsts.player.downAccel * deltaTime);
                player.anim = PlayerCharacter.Animation.Idle;
            }

            if(player.flipping == PlayerCharacter.FlipMode.Flipping && !manager.IsCrouchPressed()) {
                manager.ClearMultiJump();
            }
            if(manager.HasJumpTrigger()) {
                if(player.flipping == PlayerCharacter.FlipMode.Released) {
                    manager.DoBackflipLaunchJump();
                } else if(player.flipping == PlayerCharacter.FlipMode.Flipping) {
                    manager.DoBackflip();
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
