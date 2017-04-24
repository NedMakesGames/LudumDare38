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


using Baluga3.Core;
using UnityEngine;

namespace SmallWorld.UX {
    public class BGStar : MonoBehaviour {

        [SerializeField]
        private Color[] colors;
        [SerializeField]
        private float rotSpeedMax;
        [SerializeField]
        private float scaleMaxValue;
        [SerializeField]
        private float scaleMaxLength;

        private float rotSpeed;

        private void Start() {
            SpriteRenderer sprenderer = GetComponent<SpriteRenderer>();
            sprenderer.color = colors[Random.Range(0, colors.Length)];
            transform.rotation = Quaternion.Euler(0, 0, Random.value * 360);
            rotSpeed = (Random.value < 0.5f ? -1 : 1) * Random.value * rotSpeedMax;

            float distance = Vector2.Distance(transform.position, Vector2.zero);
            float p = distance / scaleMaxLength;
            float scale = Mathf.Lerp(1, scaleMaxValue, p * p);
            transform.localScale = new Vector3(scale, scale, scale);
        }

        private void Update() {
            transform.Rotate(0, 0, rotSpeed * Time.deltaTime);
        }
    }
}
