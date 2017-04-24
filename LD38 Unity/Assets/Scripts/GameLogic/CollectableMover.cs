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
