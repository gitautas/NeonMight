using System;
using System.Reflection;
using HarmonyLib;
using TMPro;
using UnityEngine;

// This patch skips the intro cutscene.

namespace Might
{
    public class BossHP : Patch
    {
        public override string Description => "Shows the health value of Neon Green during boss fights";

        private static string BOSSNAME_PATH = "HUD/BossUI/BossUI Anchor/BossUI Holder/Boss Name Text";
        private static string BOSSHEALTH_PATH = "HUD/BossUI/BossUI Anchor/BossUI Holder/Boss Health Text";

        private static GameObject bossHealth = null;
        private static FieldInfo _lastEnemyHealth = GetFieldInfo<BossUI>("_lastEnemyHealth");

        public override void Initialize()
        {
            {
                MethodInfo original = GetMethodInfo<BossUI>("Start");
                HarmonyMethod patch = GetHarmony<BossHP>(nameof(BossHPPatch));

                Might.NeonMight.Harmony.Patch(original, null, patch);
            }
            {
                MethodInfo original = GetMethodInfo<BossUI>("Update");
                HarmonyMethod patch = GetHarmony<BossHP>(nameof(BossHPUpdatePatch));

                Might.NeonMight.Harmony.Patch(original, patch);
            }

        }

        public static void BossHPPatch(BossUI __instance)
        {
            // if (!Preferences.BossHP) // TODO: upon disabling, unpatch the method completely.
            //     return;

            var bossName = GameObject.Find(BOSSNAME_PATH);
            bossHealth = GameObject.Find(BOSSHEALTH_PATH);

            bossHealth = UnityEngine.Object.Instantiate(bossName, bossName.transform.parent);
            bossHealth.transform.localPosition += new Vector3(3, 0, 0);
            bossHealth.SetActive(true);
        }

        public static void BossHPUpdatePatch(BossUI __instance)
        {
            int bossHP = (int)_lastEnemyHealth.GetValue(__instance);
            bossHealth.GetComponent<TextMeshPro>().SetText(bossHP.ToString());
        }
    }
}