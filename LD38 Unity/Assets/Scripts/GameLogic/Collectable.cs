using Baluga3.GameFlowLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.GameLogic {
    public enum CollectableType {
        None, Points, GameOver, Bouncy
    }

    public class Collectable {
        public PhysicsBody body;
        public CollectableType type;
        public float radius;
        public bool alive;

        public Collectable() {
            body = new PhysicsBody();
        }
    }

    public class CollectableList {
        public List<Collectable> list;
        public Message<int> onChange;

        public CollectableList() {
            list = new List<Collectable>();
            onChange = new Message<int>();
        }

        public static CollectableList Create() {
            return new CollectableList();
        }
    }
}
