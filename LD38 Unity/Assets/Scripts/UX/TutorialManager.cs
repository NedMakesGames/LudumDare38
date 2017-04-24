using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace SmallWorld.UX {
    public class TutorialManager : MonoBehaviour {

        private string[] strings = new string[] {
            "By Tim Ned Atton, live on stream at twitch.tv/nedmakesgames\nCreated for Ludum Dare 38!\nFonts by Felix Braden and Yuji Oshimoto",
            "Collect love (hearts) from the people of Earth but avoid the pink prankster asteroids!",
            "Press left and right arrow to move, C to jump, and X to crouch.",
            "Try combining jump, run, and crouch for all kinds of special moves!",
            "Collect hope (stars) from the people of Earth for a little altitude boost.\nTry jumping right after landing for more height too!",
        };

        [SerializeField]
        private float[] showPeriods;
        [SerializeField]
        private float movePeriod;
        [SerializeField]
        private float moveY;

        private int onText;
        private RectTransform anchor;
        private float timer;
        private Vector2 start;
        private Text text;
        private bool fading;

        private void Start() {
            anchor = GetComponent<RectTransform>();
            timer = 0;
            start = anchor.anchoredPosition;
            text = GetComponentInChildren<Text>();
            onText = 0;
            RefreshText();
        }

        private void Update() {
            timer += Time.deltaTime;
            if(fading) {
                anchor.anchoredPosition = Vector2.Lerp(start, new Vector2(start.x, moveY), timer / movePeriod);
                if(timer > movePeriod) {
                    gameObject.SetActive(false);
                }
            } else {
                if(timer >= showPeriods[onText]) {
                    onText++;
                    timer = 0;
                    if(onText >= strings.Length) {
                        fading = true;
                    } else {
                        RefreshText();
                    }
                }
            }
        }

        private void RefreshText() {
            text.text = strings[onText];
        }
    }
}
