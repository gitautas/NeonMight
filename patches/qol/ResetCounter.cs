// using HarmonyLib;
// using System.Reflection;

// namespace Might
// {
//     internal class ResetCounter : Patch
//     {
//         public override string Description => "Shows the amount of level attempts in-game";
//         public override void Initialize()
//         {
//             MethodInfo original = GetMethodInfo<MenuScreenMapAesthetics>("Start");
//             HarmonyMethod patch = GetHarmony<Apocalypse>(nameof(RemoveApocalypsePatch));

//             Might.NeonMight.Harmony.Patch(original, patch);
//         }

//         [HarmonyPostfix]
//         public static void RemoveApocalypsePatch(MenuScreenMapAesthetics __instance) => __instance.SetApocalypse(false); //TODO: Test this
//     }
// }