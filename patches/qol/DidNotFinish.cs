using HarmonyLib;
using System.Reflection;
using System;
using TMPro;
using UnityEngine;

namespace Might
{
    internal class DNF : Patch
    {
        public override string Description => "Shows you the possible timesave upon reaching the end of the level without meeting the level end requirements";


        private static bool timeFrozen = false;
        public override void Initialize()
        {
            MethodInfo original = GetMethodInfo<LevelGate>("OnTriggerStay");
            HarmonyMethod patch = GetHarmony<DNF>(nameof(PostOnTriggerStayPatch));

            Might.NeonMight.Harmony.Patch(original, null, patch);
        }

        public static void PostOnTriggerStayPatch(ref LevelGate __instance)
        {
            if (__instance.Unlocked || timeFrozen) return;

            timeFrozen = true;
            Game game = Singleton<Game>.Instance;

            GameObject frozenTime = UnityEngine.Object.Instantiate(RM.ui.timerText.gameObject, RM.ui.timerText.transform);

            frozenTime.transform.localPosition += new Vector3(0, 35, 0);
            long best = GameDataManager.levelStats[game.GetCurrentLevel().levelID].GetTimeBestMicroseconds();
            long curr = game.GetCurrentLevelTimerMicroseconds();

            TextMeshPro frozenText = frozenTime.GetComponent<TextMeshPro>();
            frozenText.color = best < curr ? Color.red : Color.green;

            var t = TimeSpan.FromTicks(curr * 10);
            var timeString = string.Format("{0:0}:{1:00}.{2:0}",
                t.Minutes,
                t.Seconds,
                t.Ticks / 10);

            frozenText.text = "DNF: " + timeString;
        }
    }
}