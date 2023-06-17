using System;
using System.Reflection;
using System.Linq;
using MelonLoader;

[assembly: MelonInfo(typeof(Might.NeonMight), "Neon Might", Might.NeonMight.VERSION, "gitautas")]
[assembly: MelonGame("Little Flag Software, LLC", "Neon White")]

namespace Might {
    public class NeonMight : MelonMod
    {
        public const string VERSION = "1.0";
        public static new HarmonyLib.Harmony Harmony;
        public override void OnInitializeMelon() {
            LoggerInstance.Msg("Initializing...");
            Harmony = new HarmonyLib.Harmony("NeonMight");
            InitPatches();
            LoggerInstance.Msg("Successfuly initialized.");       
        }

        public override void OnLateInitializeMelon()
        {
            InitScripts();
        }

        // This method loops over every implementation of the Patch base class, instantiates them and runs the initialization steps.
        private void InitPatches() {
            foreach (Type type in 
                Assembly.GetAssembly(typeof(Patch)).GetTypes()
                    .Where(patchType => patchType.IsClass && !patchType.IsAbstract && patchType.IsSubclassOf(typeof(Patch))))
            {
                Patch patch = (Patch)Activator.CreateInstance(type);

                patch.Initialize();
                LoggerInstance.Msg($"Initialized {type}");
            }
        }
        private void InitScripts() {
            foreach (Type type in 
                Assembly.GetAssembly(typeof(Script)).GetTypes()
                    .Where(patchType => patchType.IsClass && !patchType.IsAbstract && patchType.IsSubclassOf(typeof(Script))))
            {
                Script script = (Script)Activator.CreateInstance(type);

                script.Initialize();
                LoggerInstance.Msg($"Initialized {type}");
            }
        }
    }
}