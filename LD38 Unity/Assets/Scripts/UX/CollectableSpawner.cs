﻿// Copyright (c) 2017, Timothy Ned Atton.
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
using Baluga3.UnityCore;
using SmallWorld.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.UX {
    public class CollectableSpawner : MonoBehaviour {

        [SerializeField]
        private GameObject pointPrefab;
        [SerializeField]
        private GameObject bouncyPrefab;
        [SerializeField]
        private GameObject gameOverPrefab;

        private CollectableList clist;
        private List<CollectablePositioner> alive;

        private void Start() {
            alive = new List<CollectablePositioner>();
            clist = GameLink.TempComponents.GetOrRegister<CollectableList>((int)ComponentKeys.Collectables, CollectableList.Create);
            clist.onChange.Subscribe(new SimpleListener<int>(HandleChange));

            for(int i = 0; i < clist.list.Count; i++) {
                HandleChange(i);
            }
        }

        private void HandleChange(int index) {
            Collectable c = clist.list[index];
            GameObject gobj = null;
            foreach(var p in alive) {
                if(p.Index == index) {
                    gobj = p.gameObject;
                    break;
                }
            }
            if(gobj != null) {
                CollectAnimation.SpawnFor(c.type, c.body.cartesian, Quaternion.Euler(0, 0, -c.body.pos.x * Mathf.Rad2Deg));
                SimplePool.Despawn(gobj);
            }
            if(c.alive) {
                SpawnCollectable(index);
            }
        }

        private void SpawnCollectable(int index) {
            Collectable c = clist.list[index];
            GameObject prefab = null;
            switch(c.type) {
            case CollectableType.Points:
                prefab = pointPrefab;
                break;
            case CollectableType.Bouncy:
                prefab = bouncyPrefab;
                break;
            case CollectableType.GameOver:
                prefab = gameOverPrefab;
                break;
            }
            if(prefab == null) {
                return;
            }

            GameObject gobj = SimplePool.Spawn(prefab, c.body.cartesian, Quaternion.identity);
            CollectablePositioner posn = gobj.GetComponent<CollectablePositioner>();
            gobj.transform.parent = transform;
            posn.Setup(index);
            alive.Add(posn);
        }
    }
}
