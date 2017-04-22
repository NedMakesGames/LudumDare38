using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.GameLogic {
    public class PlayerCharacter {
        public PhysicsBody body;

        public PlayerCharacter() {
            this.body = new PhysicsBody();
        }

        public static PlayerCharacter Create() {
            return new PlayerCharacter();
        }
    }
}
