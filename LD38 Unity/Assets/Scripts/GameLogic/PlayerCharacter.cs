using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.GameLogic {
    public class PlayerCharacter {
        public enum FlipMode {
            None, Flipping, Released
        }
        public enum Animation {
            Idle, Walk, Crouch, Slide,
            Fall,
            Jump, GameOver
        }

        public PhysicsBody body;
        public int facing;
        public float multiJumpCount;
        public float multiJumpTimer;
        public int spinCount;
        public FlipMode flipping;
        //public float noGravTimer;
        public Animation anim;
        public bool hitBounce;

        public PlayerCharacter() {
            this.body = new PhysicsBody();
        }

        public static PlayerCharacter Create() {
            return new PlayerCharacter();
        }
    }
}
