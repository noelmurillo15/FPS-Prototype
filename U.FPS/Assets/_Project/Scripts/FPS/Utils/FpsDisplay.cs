/*
 * FpsDisplay - Displays current Fps to Gui
 * Created by : Allan N. Murillo
 * Last Edited : 8/24/2020
 */

using UnityEngine;

namespace ANM.Utils
{
    public class FpsDisplay : MonoBehaviour
    {
        private float _deltaTime;
        private bool _displayFps = true;


        private void Update()
        {
            if (!_displayFps) return;
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        }

        private void OnGUI()
        {
            if (!_displayFps) return;
            var style = new GUIStyle();
            int w = Screen.width, h = Screen.height;
            h *= 2 / 100;
            var rect = new Rect(w - (w * 0.5f), 0, w, h);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = Color.white;
            var msecs = _deltaTime * 1000.0f;
            var fps = 1.0f / _deltaTime;
            var text = $"{msecs:0.0} ms ({fps:0.} fps)";
            GUI.Label(rect, text, style);
        }

        public void ToggleFpsDisplay(bool b)
        {
            _displayFps = b;
        }
    }
}
