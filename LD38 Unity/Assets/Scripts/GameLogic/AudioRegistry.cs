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

using SmallWorld.UX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.GameLogic {
    public class AudioRegistry : MonoBehaviour {

        private static AudioRegistry instance;
        private static bool loaded;

        public AudioRandomizer startMove;
        public AudioRandomizer highJump;
        public AudioRandomizer lowJump;
        public AudioRandomizer switchJump;
        public AudioRandomizer spinJump;
        public AudioRandomizer backJump;
        public AudioRandomizer airJump;
        public AudioRandomizer land;
        public AudioRandomizer groundPound;
        public AudioRandomizer crouch;
        public AudioRandomizer collectHeart;
        public AudioRandomizer hitBouncy;
        public AudioRandomizer hitRock;
        public AudioSource music;

        public static AudioRegistry Create() {
            if(!loaded) {
                loaded = true;
                GameObject gobj = Instantiate(Resources.Load<GameObject>("Audio"));
                DontDestroyOnLoad(gobj);
                instance = gobj.GetComponent<AudioRegistry>();
            }
            return instance;
        }

        private void Start() {
            music.Play();
        }

        private void Update() {
            if(Input.GetButtonDown("ToggleMusic")) {
                if(music.isPlaying) {
                    music.Stop();
                } else {
                    music.Play();
                }
            }
        }
    }
}
