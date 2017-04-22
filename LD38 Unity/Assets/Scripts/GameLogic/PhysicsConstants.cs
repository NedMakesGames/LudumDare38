using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.GameLogic {
    public class PhysicsConstants : MonoBehaviour {
        public float earthRadius;
        public float gravity;
        //public Vector2 airVelDamping;
        //public Vector2 groundVelDamping;
        //public Vector2 velMinZeroing;
        //public Vector2 accelMinForVelZeroing;

        [Serializable]
        public class Player {
            public float height;

            public float snapVel;
            public float maxVel;
            public float upAccel;
            public float downAccel;
            public float axisSnapThreshhold;

            public float jumpVel;
            public float jumpHoldTime;
            public float jumpHoldSpeedMinMult;
        }
        public Player player;

        public static PhysicsConstants Create() {
            return Resources.Load<GameObject>("PhysicsConstants").GetComponent<PhysicsConstants>();
        }
    }
}
