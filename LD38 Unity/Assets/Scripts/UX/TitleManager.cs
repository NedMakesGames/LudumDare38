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
