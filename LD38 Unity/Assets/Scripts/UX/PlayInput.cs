using Baluga3.GameFlowLogic;
using SmallWorld.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.UX {
    public class PlayInput : MonoBehaviour {

        private SubscribableFloat playerAngleAxis;
        private SubscribableBool playerJump;
        private SubscribableBool playerCrouch;

        private void Start() {
            playerAngleAxis = GameLink.TempComponents.GetOrRegister<SubscribableFloat>((int)ComponentKeys.PlayerAngleAxis, SubscribableFloat.Create);
            playerJump = GameLink.TempComponents.GetOrRegister<SubscribableBool>((int)ComponentKeys.PlayerJumpBtn, SubscribableBool.Create);
            playerCrouch = GameLink.TempComponents.GetOrRegister<SubscribableBool>((int)ComponentKeys.PlayerCrouchBtn, SubscribableBool.Create);
        }

        private void Update() {
            playerAngleAxis.Value = Input.GetAxisRaw("HorizontalMovement");
            playerJump.Value = Input.GetButton("Jump");
            playerCrouch.Value = Input.GetButton("Crouch");
        }
    }
}
