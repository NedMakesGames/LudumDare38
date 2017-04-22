using SmallWorld.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.UX {
    public class PlayerSpritePositioner : MonoBehaviour {

        [SerializeField]
        private Transform playerSprite;

        private PlayerCharacter player;

        private void Start() {
            player = GameLink.TempComponents.GetOrRegister<PlayerCharacter>((int)ComponentKeys.PlayerCharacter, PlayerCharacter.Create);
        }

        private void Update() {
            playerSprite.position = player.body.cartesian;
            playerSprite.rotation = Quaternion.Euler(0, 0, -player.body.pos.x * Mathf.Rad2Deg);
        }
    }
}
