using Baluga3.GameFlowLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld.GameLogic {

    public enum GamePhase {
        Play, GameOver
    }

    public class GameState {
        public SubscribableInt score;
        public StateMachine phase;

        public GameState() {
            score = new SubscribableInt(0);
            phase = new StateMachine(0);
        }

        public static GameState Create() {
            return new GameState();
        }
    }
}
