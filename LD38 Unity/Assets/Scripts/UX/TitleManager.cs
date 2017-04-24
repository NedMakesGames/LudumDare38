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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.UX {
    public class TitleManager : MonoBehaviour {

        [SerializeField]
        private float showPeriod;
        [SerializeField]
        private float movePeriod;
        [SerializeField]
        private float moveY;

        private RectTransform anchor;
        private float timer;
        private Vector2 start;

        private void Start() {
            anchor = GetComponent<RectTransform>();
            timer = 0;
            start = anchor.anchoredPosition;
        }

        private void Update() {
            timer += Time.deltaTime;
            if(timer > showPeriod) {
                anchor.anchoredPosition = Vector2.Lerp(start, new Vector2(start.x, moveY), (timer - showPeriod) / movePeriod);
                if(timer >= showPeriod + movePeriod) {
                    gameObject.SetActive(false);
                }
            }
        }

    }
}
