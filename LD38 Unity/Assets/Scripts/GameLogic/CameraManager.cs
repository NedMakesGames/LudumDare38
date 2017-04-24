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

namespace SmallWorld.GameLogic {
    public class CameraManager : ITicking {

        private CameraPos camera;
        private PlayerCharacter player;
        private PhysicsConstants pconsts;
        private float timer;
        private float inSpeed;

        public CameraManager(AutoController ctrlr) {
            camera = ctrlr.Components.GetOrRegister<CameraPos>((int)ComponentKeys.CameraPos, CameraPos.Create);
            camera.zoom = 7.5f;
            player = ctrlr.Components.GetOrRegister<PlayerCharacter>((int)ComponentKeys.PlayerCharacter, PlayerCharacter.Create);
            pconsts = ctrlr.Game.Components.GetOrRegister<PhysicsConstants>((int)ComponentKeys.PhysicsConstants, PhysicsConstants.Create);
            ctrlr.ActivatableList.Add(new TickingJanitor(ctrlr.Game, this));
        }

        public int GetTickPriority() {
            return (int)TickPriority.Normal;
        }

        public void Tick(float deltaTime) {
            float idealZoom = Mathf.Max(pconsts.camera.minSize, pconsts.camera.abovePlayerSpace + player.body.pos.y);
            if(timer < pconsts.camera.introPeriod) {
                timer += deltaTime;
                float introPerc = timer / pconsts.camera.introPeriod;
                if(introPerc < 1) {
                    introPerc = 1f - introPerc;
                    introPerc *= introPerc;
                    introPerc = 1f - introPerc;
                }
                camera.zoom = Mathf.Lerp(
                    pconsts.camera.introStartSize,
                    idealZoom,
                    introPerc);
            } else {
                if(idealZoom >= camera.zoom) {
                    camera.zoom = idealZoom;
                    inSpeed = 0;
                } else {
                    inSpeed += pconsts.camera.zoomInAccel * deltaTime;
                    camera.zoom = Mathf.MoveTowards(camera.zoom, idealZoom, inSpeed * deltaTime);
                }
            }
            camera.rotation = player.body.pos.x;
        }
    }
}
