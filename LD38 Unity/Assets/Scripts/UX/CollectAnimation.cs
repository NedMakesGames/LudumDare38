using UnityEngine;
using System.Collections;
using Baluga3.UnityCore;
using SmallWorld.GameLogic;

namespace SmallWorld.UX {
    public class CollectAnimation : MonoBehaviour {

        private static bool loaded;
        private static GameObject prefab;

        public static void SpawnFor(CollectableType type, Vector2 pos, Quaternion rot) {
            if(!loaded) {
                loaded = true;
                prefab = Resources.Load<GameObject>("CollectAnim");
            }
            CollectAnimation canim = SimplePool.Spawn<CollectAnimation>(prefab, pos, rot);
            switch(type) {
            case CollectableType.Points:
                canim.Play("Heart");
                break;
            case CollectableType.Bouncy:
                canim.Play("Bouncy");
                break;
            case CollectableType.GameOver:
                canim.Play("Rock");
                break;
            }
        }

        [SerializeField]
        private float lifePeriod;

        private Animator anim;
        private float timer = 0;

        private void Awake() {
            anim = GetComponent<Animator>();
        }

        public void Play(string animName) {
            anim.Play(animName, 0, 0);
            timer = 0;
        }

        private void Update() {
            timer += Time.deltaTime;
            if(timer >= lifePeriod) {
                SimplePool.Despawn(gameObject);
            }
        }
    }
}
