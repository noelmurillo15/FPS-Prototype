/// <summary>
/// 
/// </summary>
using UnityEngine;


namespace FPS
{
    public class GameReferences : MonoBehaviour
    {
        public string playerTag;
        public static string _playerTag;

        public string enemyTag;
        public static string _enemyTag;

        public string npcTag;
        public static string _npcTag;

        public static GameObject myPlayer;


        private void OnEnable()
        {
            CheckTags();
            _playerTag = playerTag;
            _enemyTag = enemyTag;
            _npcTag = npcTag;
            if (_playerTag != "")
            {
                myPlayer = GameObject.FindGameObjectWithTag(_playerTag);
            }
        }

        private void CheckTags()
        {
            if (playerTag == "")
            {
                Debug.LogError("GameReferences needs a playerTag string in the inspector");
            }
            if (enemyTag == "")
            {
                Debug.LogError("GameReferences needs a enemyTag string in the inspector");
            }
            if (npcTag == "")
            {
                Debug.LogError("GameReferences needs a npcTag string in the inspector");
            }
        }
    }
}