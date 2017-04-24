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

using Baluga3.Core;
using Baluga3.GameFlowLogic;
using UnityEngine;

namespace SmallWorld.GameLogic {
    public class CollectableManager {

        private CollectableList clist;
        private GameState gameState;
        private PhysicsConstants pconsts;
        private Message<Vector2> onBouncy;
        private AudioRegistry audio;

        public CollectableManager(AutoController ctrlr) {
            clist = ctrlr.Components.GetOrRegister<CollectableList>((int)ComponentKeys.Collectables, CollectableList.Create);
            ctrlr.Components.GetOrRegister<Command<int>>((int)ComponentKeys.OnCollectableCollision, Command<int>.Create).Handler = OnCollision;

            gameState = ctrlr.Components.GetOrRegister<GameState>((int)ComponentKeys.GameState, GameState.Create);
            pconsts = ctrlr.Game.Components.GetOrRegister<PhysicsConstants>((int)ComponentKeys.PhysicsConstants, PhysicsConstants.Create);
            onBouncy = ctrlr.Components.GetOrRegister<Message<Vector2>>((int)ComponentKeys.OnBouncyCollision, Message<Vector2>.Create);
            audio = ctrlr.Components.GetOrRegister<AudioRegistry>((int)ComponentKeys.AudioRegistry, AudioRegistry.Create);

            SpawnOnScore();
        }

        private void OnCollision(int index) {
            if(gameState.phase.State == (int)GamePhase.Play) {
                Collectable collectable = clist.list[index];
                collectable.alive = false;

                clist.onChange.Send(index);

                //Debug.Log(collectable.type);

            
                switch(collectable.type) {
                case CollectableType.Points:
                    audio.collectHeart.Play();
                    gameState.score.Value += pconsts.collect.scoreAmount;
                    SpawnOnScore();
                    break;
                case CollectableType.Bouncy:
                    audio.hitBouncy.Play();
                    onBouncy.Send(collectable.body.pos);
                    break;
                case CollectableType.GameOver:
                    audio.hitRock.Play();
                    gameState.phase.State = (int)GamePhase.GameOver;
                    break;
                }
            }
        }

        private int GetNew() {
            int index = -1;
            for(int i = 0; i < clist.list.Count; i++) {
                if(!clist.list[i].alive) {
                    index = i;
                }
            }
            clist.list.Add(new Collectable());
            index = clist.list.Count - 1;

            Collectable c = clist.list[index];
            c.alive = true;
            c.spawnUncollidableTimer = 0;

            return index;
        }

        private void SpawnOnScore() {
            float score = gameState.score.Value;
            int wantedHearts = Mathf.FloorToInt(pconsts.collect.numHeartsPerScore.Evaluate(score));
            int wantedBounces = Mathf.FloorToInt(pconsts.collect.numBouncePerScore.Evaluate(score));
            int wantedRocks = Mathf.FloorToInt(pconsts.collect.numRocksPerScore.Evaluate(score));
            Range height = new Range(
                pconsts.collect.minHeightPerScore.Evaluate(score),
                pconsts.collect.maxHeightPerScore.Evaluate(score));

            int aliveHearts = 0;
            int aliveBounces = 0;
            int aliveRocks = 0;
            for(int i = 0; i < clist.list.Count; i++) {
                Collectable c = clist.list[i];
                if(c.alive) {
                    switch(c.type) {
                    case CollectableType.Points:
                        aliveHearts++;
                        break;
                    case CollectableType.Bouncy:
                        aliveBounces++;
                        break;
                    case CollectableType.GameOver:
                        aliveRocks++;
                        break;
                    }
                }
            }

            for(int i = aliveHearts; i < wantedHearts; i++) {
                int newIndex = GetNew();
                Collectable c = clist.list[newIndex];
                c.type = CollectableType.Points;
                c.movement = Random.value < pconsts.collect.heartOrbitChance.Evaluate(score) ? CollectableMoveType.Orbit : CollectableMoveType.Idle;
                c.radius = pconsts.collect.heartRadius;
                c.body.pos = FigureSpawnPos(height, true);
                clist.onChange.Send(newIndex);
            }

            for(int i = aliveBounces; i < wantedBounces; i++) {
                int newIndex = GetNew();
                Collectable c = clist.list[newIndex];
                c.type = CollectableType.Bouncy;
                c.movement = Random.value < pconsts.collect.bouncyOrbitChance.Evaluate(score) ? CollectableMoveType.Orbit : CollectableMoveType.Idle;
                c.radius = pconsts.collect.bouncyRadius;
                c.body.pos = FigureSpawnPos(height, false);
                clist.onChange.Send(newIndex);
            }

            for(int i = aliveRocks; i < wantedRocks; i++) {
                int newIndex = GetNew();
                Collectable c = clist.list[newIndex];
                c.type = CollectableType.GameOver;
                c.movement = CollectableMoveType.Orbit;
                c.radius = pconsts.collect.heartRadius;
                c.body.pos = FigureSpawnPos(height, true);
                clist.onChange.Send(newIndex);
            }
        }

        private Vector2 FigureSpawnPos(Range bounds, bool topHeavy) {
            float x = Mathf.PI * 2 * Random.value;
            float y;
            if(topHeavy) {
                y = Random.Range(Mathf.Max(bounds.Min, bounds.Max - pconsts.collect.topHeavyLeeway), bounds.Max);
            } else {
                y = Random.Range(bounds.Min, bounds.Max);
            }
            return new Vector2(x, y);
        }
    }
}
