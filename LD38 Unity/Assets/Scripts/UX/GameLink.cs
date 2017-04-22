using UnityEngine;
using System.Collections;
using System;
using SmallWorld.GameLogic;
using Baluga3.GameFlowLogic;

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

