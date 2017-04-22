using SmallWorld.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.UX {
    public class CollectablePositioner : MonoBehaviour {

        private int index;
        private CollectableList clist;

        public int Index {
            get {
                return index;
            }

            set {
                index = value;
            }
        }

        private void Start() {
            clist = GameLink.TempComponents.GetOrRegister<CollectableList>((int)ComponentKeys.Collectables, CollectableList.Create);
        }

        public void Setup(int index) {
            this.index = index;
        }

        private void Update() {
            transform.position = clist.list[index].body.cartesian;
        }
    }
}
