﻿using SmallWorld.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.UX {
    public class PlayerAnimator : MonoBehaviour {

        private Animator anim;
        private SpriteRenderer sprenderer;
        private PlayerCharacter player;

        private int lastFacing;
        private PlayerCharacter.Animation lastAnim;

        private void Start() {
            player = GameLink.TempComponents.GetOrRegister<PlayerCharacter>((int)ComponentKeys.PlayerCharacter, PlayerCharacter.Create);
            sprenderer = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();
            RefreshAnim();
            RefreshFacing();
        }

        private void Update() {
            if(player.anim != lastAnim) {
                RefreshAnim();
            }
            if(player.facing != lastFacing) {
                RefreshFacing();
            }
        }

        private void RefreshAnim() {
            lastAnim = player.anim;
            switch(lastAnim) {
            case PlayerCharacter.Animation.Idle:
                anim.Play("player idle");
                break;
            case PlayerCharacter.Animation.Walk:
                anim.Play("player walk");
                break;
            case PlayerCharacter.Animation.Crouch:
                anim.Play("player crouch");
                break;
            case PlayerCharacter.Animation.Slide:
                anim.Play("player slide");
                break;
            case PlayerCharacter.Animation.Jump:
                anim.Play("player jump");
                break;
            case PlayerCharacter.Animation.Fall:
                anim.Play("player fall");
                break;
            case PlayerCharacter.Animation.GameOver:
                anim.Play("player dead");
                break;
            }
        }

        private void RefreshFacing() {
            lastFacing = player.facing;
            sprenderer.flipX = lastFacing < 0;
        }
    }
}
