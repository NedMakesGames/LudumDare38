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
