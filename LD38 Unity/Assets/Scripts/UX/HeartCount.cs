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
