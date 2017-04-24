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
using System.Collections.Generic;
using UnityEngine;

namespace SmallWorld.GameLogic {
    public class CollectableMover : ITicking {

        private CollectableList clist;
        private Command<PhysicsBody> regPhysCmd, unregPhysCmd;
        private Query<Vector2, Vector2> toCartesian;
        private List<int> wasPhysicsRegd;
        private PhysicsConstants pconsts;
        private GameState gameState;

        public CollectableMover(AutoController ctrlr) {
            ctrlr.ActivatableList.Add(new TickingJanitor(ctrlr.Game, this));

            wasPhysicsRegd = new List<int>();

            clist = ctrlr.Components.GetOrRegister<CollectableList>((int)ComponentKeys.Collectables, CollectableList.Create);

            regPhysCmd = ctrlr.Components.GetOrRegister<Command<PhysicsBody>>((int)ComponentKeys.AddPhysicsBody, Command<PhysicsBody>.Create);
            unregPhysCmd = ctrlr.Components.GetOrRegister<Command<PhysicsBody>>((int)ComponentKeys.RemovePhysicsBody, Command<PhysicsBody>.Create);
            toCartesian = ctrlr.Components.GetOrRegister<Query<Vector2, Vector2>>((int)ComponentKeys.ToCartesian, Query<Vector2, Vector2>.Create);
            pconsts = ctrlr.Game.Components.GetOrRegister<PhysicsConstants>((int)ComponentKeys.PhysicsConstants, PhysicsConstants.Create);
            gameState = ctrlr.Components.GetOrRegister<GameState>((int)ComponentKeys.GameState, GameState.Create);

            clist.onChange.Subscribe(new SimpleListener<int>(OnCollectableChange));
            for(int i = 0; i < clist.list.Count; i++) {
                OnCollectableChange(i);
            }
        }

        public int GetTickPriority() {
            return (int)TickPriority.Physics;
        }

        public void Tick(float deltaTime) {
            for(int i = 0; i < clist.list.Count; i++) {
                Collectable c = clist.list[i];
                if(c.alive && c.movement == CollectableMoveType.Idle) {
                    c.wiggleTimer += deltaTime;
                    c.body.pos.y = c.startPosition.y + Mathf.Sin(c.wiggleTimer * pconsts.collect.wigglePeriod) * pconsts.collect.wiggleAmplitude;
                    c.body.cartesian = toCartesian.Send(c.body.pos);
                }
            }
        }

        private void OnCollectableChange(int index) {
            bool physicsRegisterable = false;
            Collectable c = clist.list[index];
            if(c.alive) {
                switch(c.movement) {
                case CollectableMoveType.Orbit:
                    physicsRegisterable = true;
                    break;
                }
            }
            bool wasReg = wasPhysicsRegd.Contains(index);
            if(physicsRegisterable && !wasReg) {
                wasPhysicsRegd.Add(index);
                regPhysCmd.Send(c.body);
            } else if(!physicsRegisterable && wasReg) {
                wasPhysicsRegd.Remove(index);
                unregPhysCmd.Send(c.body);
            }

            if(c.alive) {
                c.body.cartesian = toCartesian.Send(c.body.pos);
                
                switch(c.movement) {
                case CollectableMoveType.Orbit:
                    c.body.gravity = false;
                    c.body.vel = new Vector2(
                        (Random.value < 0.5f ? 1 : -1) * 
                        Random.Range(pconsts.collect.orbitSpeedMin.Evaluate(gameState.score.Value), 
                        pconsts.collect.orbitSpeedMax.Evaluate(gameState.score.Value)), 0);
                    break;
                case CollectableMoveType.Idle:
                    c.startPosition = c.body.pos;
                    c.wiggleTimer = 0;
                    break;
                }
            }
        }
    }
}
