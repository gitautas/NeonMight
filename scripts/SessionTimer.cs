using UnityEngine;
using UnityEngine.SceneManagement;

namespace Might
{
    public class SessionTimer : Script
    {
        public override string Description => "Displays a timer for how long the curent session has lasted";
        private Rect rect = new Rect(10, 0, 100, 70);

        public override void Initialize()
        {
            var gObject = new GameObject("SessionTimer", typeof(SessionTimer));
            UnityEngine.Object.DontDestroyOnLoad(gObject);
        }

        public void OnGUI()
        {
            GUI.Label(rect, Utils.FloatToTime(Time.realtimeSinceStartup, "#00:00"));
        }
    }
}