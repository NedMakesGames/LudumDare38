using UnityEngine;
using System.Collections;

namespace SmallWorld.UX {
    public class AudioRandomizer : MonoBehaviour {

        [SerializeField]
        private float pitchVary;
        [SerializeField]
        private float volumeVary;

        private float pitchStart;
        private float volumeStart;
        private AudioSource source;

        private void Awake() {
            source = GetComponent<AudioSource>();
            pitchStart = source.pitch;
            volumeStart = source.volume;
        }

        public void Play() {
            source.pitch = pitchStart + pitchVary * (Random.value * 2 - 1);
            source.volume = volumeStart + volumeVary * (Random.value * 2 - 1);
            source.Play();
        }
    }
}
