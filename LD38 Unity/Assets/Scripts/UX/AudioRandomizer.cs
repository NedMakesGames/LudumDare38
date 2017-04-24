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

namespace SmallWorld.UX {
    public class AudioRandomizer : MonoBehaviour {

        [SerializeField]
        private float pitchVary;
        [SerializeField]
        private float volumeVary;

        private float pitchStart;
        private float volumeStart;
        private AudioSource source;

        private void Awake() {
            source = GetComponent<AudioSource>();
            pitchStart = source.pitch;
            volumeStart = source.volume;
        }

        public void Play() {
            source.pitch = pitchStart + pitchVary * (Random.value * 2 - 1);
            source.volume = volumeStart + volumeVary * (Random.value * 2 - 1);
            source.Play();
        }
    }
}
