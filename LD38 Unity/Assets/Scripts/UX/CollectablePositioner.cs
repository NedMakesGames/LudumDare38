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

using SmallWorld.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.UX {
    public class CollectablePositioner : MonoBehaviour {

        [SerializeField]
        private float spawnPeriod;

        private int index;
        private CollectableList clist;
        private float spawnTimer;
        private Animator anim;

        public int Index {
            get {
                return index;
            }

            set {
                index = value;
            }
        }

        private void Awake() {
            anim = GetComponent<Animator>();
        }

        private void Start() {
            clist = GameLink.TempComponents.GetOrRegister<CollectableList>((int)ComponentKeys.Collectables, CollectableList.Create);
        }

        public void Setup(int index) {
            this.index = index;
            spawnTimer = 0;
            anim.Play("Spawn", 0, 0);
        }

        private void Update() {
            if(spawnTimer < spawnPeriod) {
                spawnTimer += Time.deltaTime;
                if(spawnTimer >= spawnPeriod) {
                    anim.Play("Idle");
                }
            }
            transform.position = clist.list[index].body.cartesian;
            transform.rotation = Quaternion.Euler(0, 0, -clist.list[index].body.pos.x * Mathf.Rad2Deg);
        }
    }
}
