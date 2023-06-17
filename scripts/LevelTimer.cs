using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Might
{
    public class LevelTimer : Script
    {
        public override string Description => "Displays a timer for how long the current level has been played";
        private Rect rect = new Rect(10, 20, 100, 70);
        private static float timer = 0F;

        public override void Initialize()
        {
            var gObject = new GameObject("LevelTimer", typeof(LevelTimer));
            UnityEngine.Object.DontDestroyOnLoad(gObject);
        }

        public void LevelLoaded()
        {
            MelonLoader.MelonLogger.Msg($"Scene {SceneManager.GetActiveScene().name} has been loaded!");
        }

        public void Update()
        {
            timer += Time.deltaTime;
        }

        public void OnGUI()
        {
            GUI.Label(rect, Utils.FloatToTime(timer, "#00:00"));
        }
    }
}