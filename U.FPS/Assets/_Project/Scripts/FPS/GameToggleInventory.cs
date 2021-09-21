/*
 * GameToggleInventory -
 * Created by : Allan N. Murillo
 * Last Edited : 2/2/2020
 */

using ANM.FPS;
using FPS;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ANM.Game_Management
{
    public class GameToggleInventory : MonoBehaviour
    {
        [Tooltip("Does this game mode have an Inventory? Set true if that's the case.")]
        [SerializeField] private bool hasInventory = true;
        public GameObject inventoryUi;
        
        private GameMaster _gameMaster;
        private InputAction _inventoryToggleAction;


        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _gameMaster = FindObjectOfType<GameMaster>();
            _inventoryToggleAction = new InputAction("Inventory Toggle", binding: "<Keyboard>/i");
            _inventoryToggleAction.AddBinding("<Gamepad>/select");
            _inventoryToggleAction.Enable();
            _inventoryToggleAction.performed += ToggleInventoryInput;
        }

        private void CheckCanToggle()
        {
            if (!_gameMaster.IsMenuActive && !_gameMaster.IsGameOver && hasInventory)
            {
                ToggleInventory();
            }
        }

        public void ToggleInventory()
        {
            if (inventoryUi == null) return;
            inventoryUi.SetActive(!inventoryUi.activeSelf);
            _gameMaster.IsInventoryUiActive = inventoryUi.activeSelf;
            _gameMaster.EventCallInventoryToggle();
        }
        
        private void ToggleInventoryInput(InputAction.CallbackContext context)
        {
            CheckCanToggle();
        }
        
        private void OnDisable()
        {
            _inventoryToggleAction.performed -= ToggleInventoryInput;
            _inventoryToggleAction.Disable();
        }
    }
}