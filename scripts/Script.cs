using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace Might {
    public abstract class Script : MonoBehaviour {
        public abstract string Description {get;}
        public abstract void Initialize();
    }
}