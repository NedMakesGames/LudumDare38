using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld.GameLogic {
    public class CameraPos {
        public float rotation;
        public float zoom;

        internal static CameraPos Create() {
            return new CameraPos();
        }
    }
}
