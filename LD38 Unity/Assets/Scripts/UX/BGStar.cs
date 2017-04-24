
using Baluga3.Core;
using UnityEngine;

namespace SmallWorld.UX {
    public class BGStar : MonoBehaviour {

        [SerializeField]
        private Color[] colors;
        [SerializeField]
        private float rotSpeedMax;
        [SerializeField]
        private float scaleMaxValue;
        [SerializeField]
        private float scaleMaxLength;

        private float rotSpeed;

        private void Start() {
            SpriteRenderer sprenderer = GetComponent<SpriteRenderer>();
            sprenderer.color = colors[Random.Range(0, colors.Length)];
            transform.rotation = Quaternion.Euler(0, 0, Random.value * 360);
            rotSpeed = (Random.value < 0.5f ? -1 : 1) * Random.value * rotSpeedMax;

            float distance = Vector2.Distance(transform.position, Vector2.zero);
            float p = distance / scaleMaxLength;
            float scale = Mathf.Lerp(1, scaleMaxValue, p * p);
            transform.localScale = new Vector3(scale, scale, scale);
        }

        private void Update() {
            transform.Rotate(0, 0, rotSpeed * Time.deltaTime);
        }
    }
}
