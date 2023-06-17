using HarmonyLib;
using System.Reflection;

namespace Might {
    public class RemoveMissionButton : Patch
    {
        public override string Description => "Removes the \"Start Mission\" button from the job archive";

        public override void Initialize()
        {
            MethodInfo original = typeof(MenuScreenLocation).GetMethod("CreateActionButton");
            HarmonyMethod patch = GetHarmony<RemoveMissionButton>(nameof(PreCreateActionButtonPatch));

            Might.NeonMight.Harmony.Patch(original, patch);
        }

        public static bool PreCreateActionButtonPatch(HubAction hubAction)
        {
            if (hubAction.ID == "PORTAL_CONTINUE_MISSION")
                // return !Preferences.RemoveMissionButton;
                return false;
            return true;
        }
    }
}