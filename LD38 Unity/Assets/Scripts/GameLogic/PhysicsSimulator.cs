// Copyright (c) 2017, Timothy Ned Atton.
// All rights reserved.
// nedmakesgames@gmail.com
// This code was written while streaming on twitch.tv/nedmakesgames
//
// This file is part of my Ludum Dare 38 Entry.
//
// My Ludum Dare 38 Entry is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// My Ludum Dare 38 Entry is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with My Ludum Dare 38 Entry.  If not, see <http://www.gnu.org/licenses/>.

using Baluga3.GameFlowLogic;
using SmallWorld.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SmallWorld.GameLogic {
    public class PhysicsBody {
        // Vectors are in polar coordinates. X is angle, Y is radius
        public Vector2 pos;
        public Vector2 vel;
        public Vector2 accel;
        public bool grounded;
        public Vector2 cartesian;
        public float height;
        public bool gravity;
    }

    public class PhysicsSimulator : ITicking {

        private PhysicsConstants pconsts;
        private List<PhysicsBody> bodies;

        public PhysicsSimulator(PlayController ctrlr) {
            bodies = new List<PhysicsBody>();
            pconsts = ctrlr.Game.Components.GetOrRegister<PhysicsConstants>((int)ComponentKeys.PhysicsConstants, PhysicsConstants.Create);
            ctrlr.ActivatableList.Add(new TickingJanitor(ctrlr.Game, this));

            ctrlr.Components.GetOrRegister<Command<PhysicsBody>>((int)ComponentKeys.AddPhysicsBody, Command<PhysicsBody>.Create).Handler = AddBody;
            ctrlr.Components.GetOrRegister<Command<PhysicsBody>>((int)ComponentKeys.RemovePhysicsBody, Command<PhysicsBody>.Create).Handler = RemoveBody;
            ctrlr.Components.GetOrRegister<Query<Vector2, Vector2>>((int)ComponentKeys.ToCartesian, Query<Vector2, Vector2>.Create).Handler = ToCartesian;
        }

        public int GetTickPriority() {
            return (int)TickPriority.Physics;
        }

        public void Tick(float deltaTime) {
            foreach(var body in bodies) {
                Simulate(body, deltaTime);
            }
        }

        private void Simulate(PhysicsBody body, float deltaTime) {

            body.vel += body.accel * deltaTime;
            if(body.gravity) {
                body.vel.y -= pconsts.gravity * deltaTime;
            }
            body.pos += body.vel * deltaTime;
            float minRadius = pconsts.earthRadius + body.height;
            body.grounded = body.pos.y <= minRadius;
            if(body.grounded) {
                body.pos.y = minRadius;
                body.vel.y = Mathf.Max(0, body.vel.y);
            }
            while(body.pos.x <= -Mathf.PI) {
                body.pos.x += Mathf.PI * 2;
            }
            while(body.pos.x > Mathf.PI) {
                body.pos.x -= Mathf.PI * 2;
            }
            //Vector2 damping = grounded ? pconsts.groundVelDamping : pconsts.airVelDamping;
            //vel.x *= Mathf.Pow(damping.x, Time.deltaTime);
            //vel.y *= Mathf.Pow(damping.y, Time.deltaTime);
            //if(Mathf.Abs(accel.x) < pconsts.accelMinForVelZeroing.x && Mathf.Abs(vel.x) < pconsts.velMinZeroing.x) {
            //    vel.x = 0;
            //}
            //if(Mathf.Abs(accel.y) < pconsts.accelMinForVelZeroing.y && Mathf.Abs(vel.y) < pconsts.velMinZeroing.y) {
            //    vel.y = 0;
            //}

            body.cartesian = ToCartesian(body.pos);
        }

        private Vector2 ToCartesian(Vector2 polar) {
            return new Vector2(
                Mathf.Sin(polar.x) * polar.y,
                Mathf.Cos(polar.x) * polar.y
                );
        }

        private void AddBody(PhysicsBody body) {
            bodies.Add(body);
        }

        private void RemoveBody(PhysicsBody body) {
            bodies.Add(body);
        }


    }
}
