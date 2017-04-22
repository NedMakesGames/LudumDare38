using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld.GameLogic {
    public enum TickPriority {
        Input = 0,
        Physics = 1,
        Normal = 2,
    }
}
