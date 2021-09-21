/*
 * RadialMenu - A Menu for input from a joystick (currently used for Spell Menu)
 * Created by : Allan N. Murillo
 * Last Edited : 3/25/2020
 */

using UnityEngine;

namespace ANM.FPS.Player.Input.Ui
{
    public class RadialMenu : MonoBehaviour
    {
        private int _lastLoadedSpell;
        private float _spellInputDelay;
        private float _lastRadialDegrees;
        private SpellSystem.SpellSystem _mySpellSystem;


        private void Start()
        {
            _mySpellSystem = GetComponent<SpellSystem.SpellSystem>();
        }

        private void OnGUI()
        {
            //    TODO : Use UI to depict instead of GUI
            var style = new GUIStyle();
            int w = Screen.width, h = Screen.height;
            h *= 2 / 100;
            var rect = new Rect(w - 250, 2, w, h);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = Color.white;
            var text = $"Spell Loaded : ({_lastLoadedSpell:0})";
            GUI.Label(rect, text, style);
        }

        public void CheckRadialDegrees(Vector2 axis)
        {
            if (axis == Vector2.zero) return;

            _lastRadialDegrees = Mathf.Atan2(axis.y, axis.x) * 180f / Mathf.PI;
            if (_lastRadialDegrees < 0) _lastRadialDegrees += 360f;

            var parseAxis = -1;
            if (_lastRadialDegrees >= 324 && _lastRadialDegrees <= 360)
            {
                parseAxis = 3;
            }
            else if (_lastRadialDegrees >= 0 && _lastRadialDegrees < 36)
            {
                parseAxis = 2;
            }
            else if (_lastRadialDegrees >= 36 && _lastRadialDegrees < 72)
            {
                parseAxis = 1;
            }
            else if (_lastRadialDegrees >= 72 && _lastRadialDegrees < 108)
            {
                parseAxis = 0;
            }
            else if (_lastRadialDegrees >= 108 && _lastRadialDegrees < 144)
            {
                parseAxis = 9;
            }
            else if (_lastRadialDegrees >= 144 && _lastRadialDegrees < 180)
            {
                parseAxis = 8;
            }
            else if (_lastRadialDegrees >= 180 && _lastRadialDegrees < 216)
            {
                parseAxis = 7;
            }
            else if (_lastRadialDegrees >= 216 && _lastRadialDegrees < 252)
            {
                parseAxis = 6;
            }
            else if (_lastRadialDegrees >= 252 && _lastRadialDegrees < 288)
            {
                parseAxis = 5;
            }
            else
            {
                parseAxis = 4;
            }

            _lastLoadedSpell = parseAxis;
        }

        public void CastSpell()
        {
            if (!(_spellInputDelay < Time.realtimeSinceStartup)) return;
            _spellInputDelay = Time.realtimeSinceStartup + 0.5f;
            _mySpellSystem.ActivateSpell(_lastLoadedSpell);
        }
    }
}
