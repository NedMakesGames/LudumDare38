﻿// Copyright (c) 2017, Timothy Ned Atton.
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

using System;
using System.Diagnostics;

namespace Baluga3.Core {
    public static class BalugaDebug {

        public static int tick;

        public class AssertionException : Exception {
            public AssertionException(string s) : base(s) {

            }
        }

        [Conditional("DEBUG")]
        public static void Assert(bool assertion, string message="Assertion failed") {
            if(!assertion) {
                throw new AssertionException(message);
            }
        }

        [Conditional("DEBUG")]
        public static void Log(string str) {
            UnityEngine.Debug.Log(string.Format("{0}: {1}", tick, str));
        }

        [Conditional("DEBUG")]
        public static void Log(object str) {
            Log(str == null ? "Null" : str.ToString());
        }
    }
}
