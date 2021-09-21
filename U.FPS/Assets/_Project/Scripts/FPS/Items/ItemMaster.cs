/*
 * ItemMaster -
 * Created by : Allan N. Murillo
 * Last Edited : 4/11/2020
 */

using ANM.FPS.Player;
using UnityEngine;

namespace ANM.FPS.Items
{
    public class ItemMaster : MonoBehaviour
    {
        [SerializeField] private bool isOnPlayer;
    
        public delegate void GeneralEventHandler();
        public event GeneralEventHandler EventObjectThrow;
        public event GeneralEventHandler EventObjectPickup;

        public delegate void PickupActionEventHandler(Transform item);
        public event PickupActionEventHandler EventPickupAction;
    
        private PlayerMaster _playerMaster;


        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _playerMaster = FindObjectOfType<PlayerMaster>();
            CheckIfPlayer();
        }

        private void CheckIfPlayer()
        {
            isOnPlayer = transform.root == _playerMaster.transform;
        }

        public void CallEventObjectThrow()
        {
            EventObjectThrow?.Invoke();
            if (!isOnPlayer) return;
            _playerMaster.EventCallHandsEmpty();
            _playerMaster.EventCallInventoryChange();
            CheckIfPlayer();
        }

        public void CallEventObjectPickup()
        {
            EventObjectPickup?.Invoke();
            if (isOnPlayer) return;
            _playerMaster.EventCallInventoryChange();
            CheckIfPlayer();
        }

        public void CallEventPickupAction(Transform item)
        {
            EventPickupAction?.Invoke(item);
        }
    }
}
