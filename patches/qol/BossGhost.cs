using HarmonyLib;
using System.Reflection;

namespace Might {
    public class BossfightGhost : Patch
    {
        public override string Description => "Enables recording Ghost replays for boss levels";
        private static readonly FieldInfo m_dontRecord = GetFieldInfo<GhostRecorder>("m_dontRecord");
        public override void Initialize()
        {
            {
                MethodInfo original = GetMethodInfo<GhostRecorder>("Start");
                HarmonyMethod patch = GetHarmony<BossfightGhost>(nameof(RecordGhostPatch));

                Might.NeonMight.Harmony.Patch(original, patch);
            }
            {
                MethodInfo original = GetMethodInfo<GhostPlayback>("LateUpdate");
                HarmonyMethod patch = GetHarmony<BossfightGhost>(nameof(PreLateUpdatePatch));

                Might.NeonMight.Harmony.Patch(original, patch);
            }

        }

        public static bool RecordGhostPatch(GhostRecorder __instance)
        {
            RM.ghostRecorder = __instance;
            if (LevelRush.IsHellRush())
                m_dontRecord.SetValue(__instance, true);
            return false;
        }

        public static bool PreLateUpdatePatch(GhostPlayback __instance)
        {
            if (Preferences.RecordBossGhost)
                return true;

            // if (Singleton<Game>.Instance.GetCurrentLevel().isBossFight || LevelRush.IsHellRush())
            //     return false;

            return true;
        }
    }
}