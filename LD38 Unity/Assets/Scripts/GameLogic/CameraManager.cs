using Baluga3.GameFlowLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld.GameLogic {
    public class CameraManager : ITicking {

        private CameraPos camera;
        private PlayerCharacter player;

        public CameraManager(AutoController ctrlr) {
            camera = ctrlr.Components.GetOrRegister<CameraPos>((int)ComponentKeys.CameraPos, CameraPos.Create);
            camera.zoom = 7.5f;
            player = ctrlr.Components.GetOrRegister<PlayerCharacter>((int)ComponentKeys.PlayerCharacter, PlayerCharacter.Create);
            ctrlr.ActivatableList.Add(new TickingJanitor(ctrlr.Game, this));
        }

        public int GetTickPriority() {
            return (int)TickPriority.Normal;
        }

        public void Tick(float deltaTime) {
            camera.rotation = player.body.pos.x;
        }
    }
}
