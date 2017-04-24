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
using SmallWorld.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.UX {
    public class PlayInput : MonoBehaviour {

        private SubscribableFloat playerAngleAxis;
        private SubscribableBool playerJump;
        private SubscribableBool playerCrouch;

        private void Start() {
            playerAngleAxis = GameLink.TempComponents.GetOrRegister<SubscribableFloat>((int)ComponentKeys.PlayerAngleAxis, SubscribableFloat.Create);
            playerJump = GameLink.TempComponents.GetOrRegister<SubscribableBool>((int)ComponentKeys.PlayerJumpBtn, SubscribableBool.Create);
            playerCrouch = GameLink.TempComponents.GetOrRegister<SubscribableBool>((int)ComponentKeys.PlayerCrouchBtn, SubscribableBool.Create);
        }

        private void Update() {
            playerAngleAxis.Value = Input.GetAxisRaw("HorizontalMovement");
            playerJump.Value = Input.GetButton("Jump");
            playerCrouch.Value = Input.GetButton("Crouch");
        }
    }
}
