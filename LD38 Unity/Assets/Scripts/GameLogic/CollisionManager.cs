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
