/*
 * GameMaster -
 * Created by : Allan N. Murillo
 * Last Edited : 4/11/2020
 */

using UnityEngine;

namespace ANM.FPS
{
    public class GameMaster : MonoBehaviour
    {
        public delegate void GameMasterEventHandler();

        public event GameMasterEventHandler MenuToggleEvent;
        public event GameMasterEventHandler InventoryToggleEvent;
        public event GameMasterEventHandler GameOverEvent;

        [SerializeField] private bool isGameOver = false;
        [SerializeField] private bool isInventoryUiActive = false;
        [SerializeField] private bool isMenuActive = false;

        public bool IsGameOver
        {
            get => isGameOver;
            set => isGameOver = value;
        }

        public bool IsInventoryUiActive
        {
            get => isInventoryUiActive;
            set => isInventoryUiActive = value;
        }

        public bool IsMenuActive
        {
            get => isMenuActive;
            set => isMenuActive = value;
        }


        public void EventCallMenuToggle()
        {
            MenuToggleEvent?.Invoke();
        }

        public void EventCallInventoryToggle()
        {
            InventoryToggleEvent?.Invoke();
        }

        public void EventCallGameOver()
        {
            if (GameOverEvent == null) return;
            if (isGameOver) return;
            isGameOver = true;
            GameOverEvent();
        }
    }
}
