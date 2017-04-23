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
            public float jumpHorizAccel;
            public float landingMultiJumpPeriod;
            public float jumpTriggerLeeway;
            public float jumpHeightScaling;

            public float crouchMaxVel;
            public float crouchAccel;
            public float slideAccel;
            public float backflipJumpVel;
            public float backflipHorizVel;
            public float backflipHorizScaling;

            public float switchJumpVel;

            public float spinJumpVel;
            public float spinJumpPeriod;

            public float lowJumpVel;
            public float lowJumpHorizVel;

            public float backflipLaunchJumpVel;
            public float airPoundLeeway;
            public float airJumpCooldown;
            public float airJumpDrag;
            public float airJumpSpeed;
            public int groundPoundSpeed;
            public float groundPoundHorizAccel;
        }
        public Player player;

        [Serializable]
        public class Collectables {
            public int scoreAmount;
        }
        public Collectables collect;

        public static PhysicsConstants Create() {
            return Resources.Load<GameObject>("PhysicsConstants").GetComponent<PhysicsConstants>();
        }
    }
}
