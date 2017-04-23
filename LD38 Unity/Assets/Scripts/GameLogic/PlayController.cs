using Baluga3.GameFlowLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld.GameLogic {
    public class PlayController : AutoController {
        public PlayController(Game game) : base(game) {

            new PhysicsSimulator(this);
            new PlayerMoveManager(this);
            new CameraManager(this);
            new CollectableManager(this);
            new CollisionManager(this);
            new CollectableMover(this);
        }
    }
}
