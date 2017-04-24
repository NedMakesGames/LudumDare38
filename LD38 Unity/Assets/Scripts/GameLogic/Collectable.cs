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

    public enum CollectableMoveType {
        Idle, Orbit, 
    }

    public class Collectable {
        public PhysicsBody body;
        public CollectableType type;
        public CollectableMoveType movement;
        public float radius;
        public bool alive;
        public float spawnUncollidableTimer;
        public Vector2 startPosition;
        public float wiggleTimer;

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
