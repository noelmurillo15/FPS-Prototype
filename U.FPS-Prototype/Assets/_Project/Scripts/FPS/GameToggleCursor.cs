using UnityEngine;

namespace ANM.Game_Management
{
    public class GameToggleCursor : MonoBehaviour
    {
        [SerializeField] private bool isCursorLocked = true;

        
        private void OnEnable()
        {
            SetCursorLocked(true);
        }

        public void SetCursorLocked(bool toggle){
            isCursorLocked = toggle;
            CheckCursorLocked();
        }

        private void CheckCursorLocked()
        {
            if (isCursorLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        private void OnDestroy()
        {
            SetCursorLocked(false);
        }
    }
}