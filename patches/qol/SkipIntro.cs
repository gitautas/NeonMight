using HarmonyLib;
using System.Reflection;

namespace Might
{
    public class SkipIntro : Patch
    {
        public override string Description => "Skips the intro cinematic when launching the game";

        public override void Initialize()
        {
            MethodInfo original = GetMethodInfo<IntroCards>("Start");
            HarmonyMethod patch = GetHarmony<SkipIntro>(nameof(IntroCardsPatch));

            Might.NeonMight.Harmony.Patch(original, null, patch);
        }

        public static void IntroCardsPatch(IntroCards __instance)
        {
            __instance.introCards = new IntroCard[0];
        }
    }
}