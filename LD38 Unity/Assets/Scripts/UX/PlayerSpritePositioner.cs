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

using SmallWorld.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.UX {
    public class PlayerSpritePositioner : MonoBehaviour {

        [SerializeField]
        private Transform playerSprite;

        private PlayerCharacter player;

        private void Start() {
            player = GameLink.TempComponents.GetOrRegister<PlayerCharacter>((int)ComponentKeys.PlayerCharacter, PlayerCharacter.Create);
        }

        private void Update() {
            playerSprite.position = player.body.cartesian;
            playerSprite.rotation = Quaternion.Euler(0, 0, -player.body.pos.x * Mathf.Rad2Deg);
        }
    }
}
