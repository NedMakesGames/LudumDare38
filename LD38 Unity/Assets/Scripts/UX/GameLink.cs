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
using System;
using SmallWorld.GameLogic;
using Baluga3.GameFlowLogic;
using UnityEngine.SceneManagement;

namespace SmallWorld.UX {
    public class GameLink : MonoBehaviour {

        public enum LoadMode {
            Unknown = 0, Play
        }

        private static bool loaded = false;
        private static GameLink instance;

        public static void Load(LoadMode mode) {
            if(!loaded) {
                loaded = true;
                instance = Baluga3.UnityCore.Baluga3Object.GetOrAdd<GameLink>();
                instance.LoadInstance(mode);
            }
        }

        public static SmallWorldGame Game {
            get {
                Load(LoadMode.Unknown);
                return instance.game;
            }
        }

        public static ComponentRegistry TempComponents {
            get {
                Load(LoadMode.Unknown);
                return ((AutoController)instance.game.Controller).Components;
            }
        }

        public static void Restart() {
            SceneManager.LoadScene("Play");
            instance.LoadInstance(LoadMode.Play);
        }

        private SmallWorldGame game;

        private void LoadInstance(LoadMode mode) {
            game = new SmallWorldGame();
            switch(mode) {
            case LoadMode.Play:
                game.Controller = new PlayController(game);
                break;
            default:
                throw new ArgumentException("GameLink cannot load game in Unknown load mode");
            }
        }

        private void Update() {
            game.Tick(Time.deltaTime);
        }
    }
}

