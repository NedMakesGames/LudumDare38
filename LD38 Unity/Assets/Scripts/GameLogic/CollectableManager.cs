using Baluga3.GameFlowLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.GameLogic {
    public class CollectableManager {

        private CollectableList clist;
        private GameState gameState;
        private PhysicsConstants pconsts;
        private Message<Vector2> onBouncy;

        public CollectableManager(AutoController ctrlr) {
            clist = ctrlr.Components.GetOrRegister<CollectableList>((int)ComponentKeys.Collectables, CollectableList.Create);
            ctrlr.Components.GetOrRegister<Command<int>>((int)ComponentKeys.OnCollectableCollision, Command<int>.Create).Handler = OnCollision;

            gameState = ctrlr.Components.GetOrRegister<GameState>((int)ComponentKeys.GameState, GameState.Create);
            pconsts = ctrlr.Game.Components.GetOrRegister<PhysicsConstants>((int)ComponentKeys.PhysicsConstants, PhysicsConstants.Create);
            onBouncy = ctrlr.Components.GetOrRegister<Message<Vector2>>((int)ComponentKeys.OnBouncyCollision, Message<Vector2>.Create);

            Collectable c = new Collectable();
            clist.list.Add(c);
            c.body.pos = new UnityEngine.Vector2(0, 5);
            c.type = CollectableType.Bouncy;
            c.movement = CollectableMoveType.Orbit;
            c.radius = 0.5f;
            c.alive = true;

            //regCmd.Send(c.body);
        }

        private void OnCollision(int index) {
            Collectable collectable = clist.list[index];
            collectable.alive = false;

            clist.onChange.Send(index);

            Debug.Log(collectable.type);

            switch(collectable.type) {
            case CollectableType.Points:
                gameState.score.Value += pconsts.collect.scoreAmount;
                break;
            case CollectableType.Bouncy:
                onBouncy.Send(collectable.body.pos);
                break;
            case CollectableType.GameOver:
                gameState.phase.State = (int)GamePhase.GameOver;
                break;
            }
        }
    }
}
