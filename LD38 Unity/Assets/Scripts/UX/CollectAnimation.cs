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

using UnityEngine;
using System.Collections;
using Baluga3.UnityCore;
using SmallWorld.GameLogic;

namespace SmallWorld.UX {
    public class CollectAnimation : MonoBehaviour {

        private static bool loaded;
        private static GameObject prefab;

        public static void SpawnFor(CollectableType type, Vector2 pos, Quaternion rot) {
            if(!loaded) {
                loaded = true;
                prefab = Resources.Load<GameObject>("CollectAnim");
            }
            CollectAnimation canim = SimplePool.Spawn<CollectAnimation>(prefab, pos, rot);
            switch(type) {
            case CollectableType.Points:
                canim.Play("Heart");
                break;
            case CollectableType.Bouncy:
                canim.Play("Bouncy");
                break;
            case CollectableType.GameOver:
                canim.Play("Rock");
                break;
            }
        }

        [SerializeField]
        private float lifePeriod;

        private Animator anim;
        private float timer = 0;

        private void Awake() {
            anim = GetComponent<Animator>();
        }

        public void Play(string animName) {
            anim.Play(animName, 0, 0);
            timer = 0;
        }

        private void Update() {
            timer += Time.deltaTime;
            if(timer >= lifePeriod) {
                SimplePool.Despawn(gameObject);
            }
        }
    }
}
