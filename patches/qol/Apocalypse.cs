using HarmonyLib;
using System.Reflection;

namespace Might
{
    internal class Apocalypse : Patch
    {
        public override string Description => "Changes the color of the main menu map screen from the offputting green tint";
        public override void Initialize()
        {
            MethodInfo original = GetMethodInfo<MenuScreenMapAesthetics>("Start");
            HarmonyMethod patch = GetHarmony<Apocalypse>(nameof(RemoveApocalypsePatch));

            Might.NeonMight.Harmony.Patch(original, null, patch);
        }

        public static void RemoveApocalypsePatch(MenuScreenMapAesthetics __instance) => __instance.SetApocalypse(false); //TODO: Test this
    }
}