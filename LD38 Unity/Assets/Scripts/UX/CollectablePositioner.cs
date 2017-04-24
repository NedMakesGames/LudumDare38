using SmallWorld.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.UX {
    public class CollectablePositioner : MonoBehaviour {

        [SerializeField]
        private float spawnPeriod;

        private int index;
        private CollectableList clist;
        private float spawnTimer;
        private Animator anim;

        public int Index {
            get {
                return index;
            }

            set {
                index = value;
            }
        }

        private void Awake() {
            anim = GetComponent<Animator>();
        }

        private void Start() {
            clist = GameLink.TempComponents.GetOrRegister<CollectableList>((int)ComponentKeys.Collectables, CollectableList.Create);
        }

        public void Setup(int index) {
            this.index = index;
            spawnTimer = 0;
            anim.Play("Spawn", 0, 0);
        }

        private void Update() {
            if(spawnTimer < spawnPeriod) {
                spawnTimer += Time.deltaTime;
                if(spawnTimer >= spawnPeriod) {
                    anim.Play("Idle");
                }
            }
            transform.position = clist.list[index].body.cartesian;
            transform.rotation = Quaternion.Euler(0, 0, -clist.list[index].body.pos.x * Mathf.Rad2Deg);
        }
    }
}
