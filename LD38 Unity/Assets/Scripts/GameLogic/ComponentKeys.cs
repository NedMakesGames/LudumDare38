using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld.GameLogic {
    public enum ComponentKeys {
        None,
        PhysicsConstants,
        PlayerCharacter,
        AddPhysicsBody,
        RemovePhysicsBody,
        CameraPos,
        PlayerAngleAxis,
        PlayerJumpBtn,
        Collectables,
        ToCartesian,
        OnCollectableCollision,
        PlayerCrouchBtn,
        GameState,
        OnBouncyCollision,
    }
}
