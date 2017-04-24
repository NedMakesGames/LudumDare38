using SmallWorld.UX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.GameLogic {
    public class AudioRegistry : MonoBehaviour {

        private static AudioRegistry instance;
        private static bool loaded;

        public AudioRandomizer startMove;
        public AudioRandomizer highJump;
        public AudioRandomizer lowJump;
        public AudioRandomizer switchJump;
        public AudioRandomizer spinJump;
        public AudioRandomizer backJump;
        public AudioRandomizer airJump;
        public AudioRandomizer land;
        public AudioRandomizer groundPound;
        public AudioRandomizer crouch;
        public AudioRandomizer collectHeart;
        public AudioRandomizer hitBouncy;
        public AudioRandomizer hitRock;
        public AudioSource music;

        public static AudioRegistry Create() {
            if(!loaded) {
                loaded = true;
                GameObject gobj = Instantiate(Resources.Load<GameObject>("Audio"));
                DontDestroyOnLoad(gobj);
                instance = gobj.GetComponent<AudioRegistry>();
            }
            return instance;
        }

        private void Start() {
            music.Play();
        }

        private void Update() {
            if(Input.GetButtonDown("ToggleMusic")) {
                if(music.isPlaying) {
                    music.Stop();
                } else {
                    music.Play();
                }
            }
        }
    }
}
