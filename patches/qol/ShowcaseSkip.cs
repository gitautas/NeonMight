using System;
using HarmonyLib;
using System.Reflection;

namespace Might {
    public class ShowcaseSkip : Patch
    {
        public override string Description => "Skips all tutorial cards.";

        public override void Initialize()
        {
            MethodInfo original = typeof(MainMenu).GetMethod("SetItemShowcaseCard");

            HarmonyMethod patch = GetHarmony<ShowcaseSkip>(nameof(PreSetItemShowcaseCardPatch));

            Might.NeonMight.Harmony.Patch(original, patch);
        }

        public static bool PreSetItemShowcaseCardPatch(MainMenu __instance, ref PlayerCardData cardData, ref Action callback)
        {
            // if (!Preferences.ShowcaseSkip)
            //     return true;
            MelonLoader.MelonLogger.Msg("penis");
            callback?.Invoke();
            return false;
        }
    }
}