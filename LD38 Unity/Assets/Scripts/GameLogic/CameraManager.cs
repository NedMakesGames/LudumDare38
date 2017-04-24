using Baluga3.GameFlowLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.GameLogic {
    public class CameraManager : ITicking {

        private CameraPos camera;
        private PlayerCharacter player;
        private PhysicsConstants pconsts;
        private float timer;
        private float inSpeed;

        public CameraManager(AutoController ctrlr) {
            camera = ctrlr.Components.GetOrRegister<CameraPos>((int)ComponentKeys.CameraPos, CameraPos.Create);
            camera.zoom = 7.5f;
            player = ctrlr.Components.GetOrRegister<PlayerCharacter>((int)ComponentKeys.PlayerCharacter, PlayerCharacter.Create);
            pconsts = ctrlr.Game.Components.GetOrRegister<PhysicsConstants>((int)ComponentKeys.PhysicsConstants, PhysicsConstants.Create);
            ctrlr.ActivatableList.Add(new TickingJanitor(ctrlr.Game, this));
        }

        public int GetTickPriority() {
            return (int)TickPriority.Normal;
        }

        public void Tick(float deltaTime) {
            float idealZoom = Mathf.Max(pconsts.camera.minSize, pconsts.camera.abovePlayerSpace + player.body.pos.y);
            if(timer < pconsts.camera.introPeriod) {
                timer += deltaTime;
                float introPerc = timer / pconsts.camera.introPeriod;
                if(introPerc < 1) {
                    introPerc = 1f - introPerc;
                    introPerc *= introPerc;
                    introPerc = 1f - introPerc;
                }
                camera.zoom = Mathf.Lerp(
                    pconsts.camera.introStartSize,
                    idealZoom,
                    introPerc);
            } else {
                if(idealZoom >= camera.zoom) {
                    camera.zoom = idealZoom;
                    inSpeed = 0;
                } else {
                    inSpeed += pconsts.camera.zoomInAccel * deltaTime;
                    camera.zoom = Mathf.MoveTowards(camera.zoom, idealZoom, inSpeed * deltaTime);
                }
            }
            camera.rotation = player.body.pos.x;
        }
    }
}
