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
    public class CollisionManager : ITicking {

        private PlayerCharacter player;
        private CollectableList collectables;
        private Command<int> onCollision;
        private PhysicsConstants pconsts;

        public CollisionManager(AutoController ctrlr) {
            player = ctrlr.Components.GetOrRegister<PlayerCharacter>((int)ComponentKeys.PlayerCharacter, PlayerCharacter.Create);
            collectables = ctrlr.Components.GetOrRegister<CollectableList>((int)ComponentKeys.Collectables, CollectableList.Create);
            onCollision = ctrlr.Components.GetOrRegister<Command<int>>((int)ComponentKeys.OnCollectableCollision, Command<int>.Create);
            pconsts = ctrlr.Game.Components.GetOrRegister<PhysicsConstants>((int)ComponentKeys.PhysicsConstants, PhysicsConstants.Create);
            ctrlr.ActivatableList.Add(new TickingJanitor(ctrlr.Game, this));
        }

        public int GetTickPriority() {
            return (int)TickPriority.Normal;
        }

        public void Tick(float deltaTime) {
            Vector2 ppos = player.body.cartesian;
            for(int c = 0; c < collectables.list.Count ; c++) {
                Collectable collectable = collectables.list[c];
                if(collectable.alive) {
                    collectable.spawnUncollidableTimer += deltaTime;
                    if(collectable.spawnUncollidableTimer >= pconsts.collect.spawnUncollidablePeriod) {
                        Vector2 cpos = collectable.body.cartesian;
                        if((ppos - cpos).sqrMagnitude <= collectable.radius * collectable.radius) {
                            onCollision.Send(c);
                        }
                    }
                }
            }
        }
    }
}
