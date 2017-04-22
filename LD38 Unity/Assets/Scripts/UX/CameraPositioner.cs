using SmallWorld.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.UX {
    public class CameraPositioner : MonoBehaviour {

        [SerializeField]
        private new Camera camera;

        private CameraPos pos;

        private void Start() {
            pos = GameLink.TempComponents.GetOrRegister<CameraPos>((int)ComponentKeys.CameraPos, CameraPos.Create);
        }

        private void Update() {
            transform.rotation = Quaternion.Euler(0, 0, -pos.rotation * Mathf.Rad2Deg);
            camera.orthographicSize = pos.zoom;
        }
    }
}
