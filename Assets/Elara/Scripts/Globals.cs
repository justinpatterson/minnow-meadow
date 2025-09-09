using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elara.Settings {
    public static class Globals
    {
        public enum Difficulty { Easy, Medium, Hard }
        public static Difficulty DifficultyState = Difficulty.Medium;
    }
}