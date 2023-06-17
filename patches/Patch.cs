using System;
using System.Reflection;
using HarmonyLib;

namespace Might {
    public abstract class Patch {
        public abstract string Description {get;}
        public abstract void Initialize();
        protected static HarmonyMethod GetHarmony<T>(String methodName) {
            return new HarmonyMethod(typeof(T).GetMethod(methodName));
        }
        protected static MethodInfo GetMethodInfo<T>(String methodName) {
            return typeof(T).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        }

        protected static FieldInfo GetFieldInfo<T>(String fieldName) {
            return typeof(T).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }
}