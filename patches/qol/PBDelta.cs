using System;
using System.Reflection;
using System.Text;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace Might
{
    public class PBDelta : Patch
    {
        public override string Description => "Displays a colored time delta comparison to your personal best after completing a level";

        private static string delta = string.Empty;
        private static TextMeshProUGUI text = null;
        private static bool newBest = false;

        private static string NEWBEST_TEXT_PATH = "Main Menu/Canvas/Ingame Menu/Menu Holder/Results Panel/New Best Text";
        private static string DELTATIME_PATH = "Main Menu/Canvas/Ingame Menu/Menu Holder/Results Panel/Delta Time";

        private static string NEWBEST_RUSH_PATH = "Main Menu/Canvas/Ingame Menu/Menu Holder/Level Rush Complete Panel/Level Time Text";
        private static string DELTATIME_RUSH_PATH = "Main Menu/Canvas/Ingame Menu/Menu Holder/Level Rush Complete Panel/Delta Time Rush";

        private static FieldInfo _currentPlaythrough = GetFieldInfo<Game>("_currentPlaythrough");

        public override void Initialize()
        {
            {
                MethodInfo original = typeof(Game).GetMethod("OnLevelWin");
                HarmonyMethod patch = GetHarmony<PBDelta>(nameof(PreOnLevelWin));

                Might.NeonMight.Harmony.Patch(original, patch);
            }
            {
                MethodInfo original = typeof(MenuScreenResults).GetMethod("OnSetVisible");
                HarmonyMethod patch = GetHarmony<PBDelta>(nameof(PostOnSetVisible));

                Might.NeonMight.Harmony.Patch(original, null, patch);
            }
            {
                MethodInfo original = typeof(MenuScreenResults).GetMethod("OnSetInvisible");
                HarmonyMethod patch = GetHarmony<PBDelta>(nameof(Destroy));

                Might.NeonMight.Harmony.Patch(original, null, patch);
            }
        }

        public static bool PreOnLevelWin()
        {
            // if (!Preferences.PBTracker)
            //     return true;

            if (LevelRush.IsLevelRush()) return true;
            delta = GetDeltaTimeString();
            return true;
        }

        public static void PostOnSetVisible()
        {
            // if (!Preferences.PBTracker)
            // return;

            int yPos = newBest ? -20 : -30;
            int xPos = LevelRush.IsLevelRush() ? -5 : 0;

            Color color = newBest ? Color.red : Color.green;

            var bestText = GameObject.Find(NEWBEST_TEXT_PATH);
            var deltaTime = GameObject.Find(DELTATIME_PATH);

            if (LevelRush.IsLevelRush())
            {
                bestText = GameObject.Find(NEWBEST_RUSH_PATH);
                deltaTime = GameObject.Find(DELTATIME_RUSH_PATH);
            }

            deltaTime = UnityEngine.Object.Instantiate(bestText, bestText.transform.parent);
            deltaTime.transform.localPosition += new Vector3(xPos, yPos, 0);
            deltaTime.SetActive(true);

            text = deltaTime.GetComponent<TextMeshProUGUI>();
            text.SetText(delta);
            text.color = color;
        }

        private static string GetDeltaTimeString()
        {
            var game = Singleton<Game>.Instance;

            long bestTime, newTime;

            if (!LevelRush.IsLevelRush())
            { // Normal level
                var levelInformation = game.GetGameData().GetLevelInformation(game.GetCurrentLevel());
                bestTime = GameDataManager.levelStats[levelInformation.levelID].GetTimeBestMicroseconds();
                var currentPlaythrough = (LevelPlaythrough)_currentPlaythrough.GetValue(game);
                newTime = currentPlaythrough.GetCurrentTimeMicroseconds();
            }
            else
            { // Level Rush
                var bestLevelRushData = LevelRush.GetLevelRushDataByType(LevelRush.GetCurrentLevelRushType());
                bestTime = LevelRush.IsHellRush() ? bestLevelRushData.bestTime_HellMicroseconds : bestLevelRushData.bestTime_HeavenMicroseconds;
                newTime = LevelRush.GetCurrentLevelRushTimerMicroseconds();
            }

            long deltaTime = (bestTime - newTime);
            newBest = deltaTime < 0;
            var t = TimeSpan.FromTicks(Math.Abs(deltaTime * 10));

            return (newBest ? "+" : "-") + 
                string.Format("{0:0}:{1:00}.{2:0}",
                    t.Minutes,
                    t.Seconds,
                    t.Ticks / 10);
        }

        public static void Destroy() => UnityEngine.Object.Destroy(text);
    }
}