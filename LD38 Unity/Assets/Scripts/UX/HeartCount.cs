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
using SmallWorld.GameLogic;
using UnityEngine.UI;
using Baluga3.GameFlowLogic;

namespace SmallWorld.UX {
    public class HeartCount : MonoBehaviour {

        [SerializeField]
        private Text scoreText;
        [SerializeField]
        private Image bgImage;

        private GameState state;

        private void Start() {
            scoreText.gameObject.SetActive(false);
            bgImage.gameObject.SetActive(false);
            state = GameLink.TempComponents.GetOrRegister<GameState>((int)ComponentKeys.GameState, GameState.Create);
            state.score.Subscribe(new SimpleListener<int>((s) => OnScoreChange()));
        }

        private void OnScoreChange() {
            scoreText.text = string.Format("{0}", state.score.Value);
            scoreText.gameObject.SetActive(state.score.Value > 0);
            bgImage.gameObject.SetActive(state.score.Value > 0);
        }
    }
}
