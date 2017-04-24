using UnityEngine;
using System.Collections;
using SmallWorld.GameLogic;
using Baluga3.GameFlowLogic;
using System;

namespace SmallWorld.UX {
    public class GameOverManager : MonoBehaviour {

        [SerializeField]
        private float movePeriod;
        [SerializeField]
        private float startY;

        private RectTransform anchor;
        private float timer;
        private Vector2 start;

        private void Start() {
            anchor = GetComponent<RectTransform>();
            timer = 0;
            start = anchor.anchoredPosition;
            anchor.anchoredPosition = new Vector2(start.x, startY);

            GameLink.TempComponents.GetOrRegister<GameState>((int)ComponentKeys.GameState, GameState.Create).phase.EnterStateMessenger
                .Subscribe(new SimpleListener<int>(OnPhaseChange));
            gameObject.SetActive(false);
        }

        private void OnPhaseChange(int phase) {
            if(phase == (int)GamePhase.GameOver) {
                gameObject.SetActive(true);
            }
        }

        private void Update() {
            timer += Time.deltaTime;
            anchor.anchoredPosition = Vector2.Lerp(new Vector2(start.x, startY), start, timer / movePeriod);
            if(timer >= movePeriod && Input.GetButtonDown("Restart")) {
                GameLink.Restart();
            }
        }
    }
}
