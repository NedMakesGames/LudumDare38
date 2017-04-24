using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.GameLogic.PlayerStates {
    class GameOver : PlayerMoveManager.StateController {

        public GameOver(PlayerMoveManager manager) : base(manager) {

        }

        public override void Enter() {
            player.body.vel = Vector2.zero;
            player.body.height = pconsts.player.crouchHeight;
            //player.body.gravity = true;
            player.anim = PlayerCharacter.Animation.GameOver;
        }

        public override void Tick(float deltaTime) {

        }
    }
}
