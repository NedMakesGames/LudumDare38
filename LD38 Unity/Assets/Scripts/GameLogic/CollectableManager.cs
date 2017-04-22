using Baluga3.GameFlowLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.GameLogic {
    public class CollectableManager {

        private CollectableList clist;
        private Command<PhysicsBody> regCmd;

        public CollectableManager(AutoController ctrlr) {
            clist = ctrlr.Components.GetOrRegister<CollectableList>((int)ComponentKeys.Collectables, CollectableList.Create);
            regCmd = ctrlr.Components.GetOrRegister<Command<PhysicsBody>>((int)ComponentKeys.AddPhysicsBody, Command<PhysicsBody>.Create);
            ctrlr.Components.GetOrRegister<Command<int>>((int)ComponentKeys.OnCollectableCollision, Command<int>.Create).Handler = OnCollision;

            Collectable c = new Collectable();
            clist.list.Add(c);
            c.body.cartesian = new UnityEngine.Vector2(0, -3);
            c.type = CollectableType.Points;
            c.radius = 0.5f;
            c.alive = true;

            //regCmd.Send(c.body);
        }

        private void OnCollision(int index) {
            Collectable collectable = clist.list[index];
            collectable.alive = false;

            clist.onChange.Send(index);

            switch(collectable.type) {
            case CollectableType.Points:

                break;
            }
        }
    }
}
