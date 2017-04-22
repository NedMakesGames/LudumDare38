using UnityEngine;

namespace SmallWorld.UX {
    public class GameLinkLoader : MonoBehaviour {

        [SerializeField]
        private GameLink.LoadMode loadMode;

        private void Awake() {
            GameLink.Load(loadMode);
        }
    }
}
