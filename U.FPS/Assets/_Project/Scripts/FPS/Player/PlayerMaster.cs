/*
 * PlayerMaster -
 * Created By : Allan Murillo
 * Last Edited : 4/11/2020
 */

using UnityEngine;

namespace ANM.FPS.Player
{
    public class PlayerMaster : MonoBehaviour
    {
        public Transform playerCam;

        public delegate void GeneralEventHandler();
        public delegate void PlayerHpEventHandler(float hp);
        public delegate void AmmoEventHandler(string ammoName, int quantity);

        public event AmmoEventHandler PickUpAmmoEvent;
        public event GeneralEventHandler AmmoSwapEvent;
        public event GeneralEventHandler PlayerDieEvent;
        public event GeneralEventHandler HandsEmptyEvent;
        public event GeneralEventHandler InventoryChangeEvent;
        public event PlayerHpEventHandler PlayerHpIncreaseEvent;
        public event PlayerHpEventHandler PlayerHpDeductionEvent;

    
        public void EventCallPlayerDie()
        {
            PlayerDieEvent?.Invoke();
        }

        public void EventCallInventoryChange()
        {
            InventoryChangeEvent?.Invoke();
        }

        public void EventCallHandsEmpty()
        {
            HandsEmptyEvent?.Invoke();
        }

        public void EventCallAmmoSwap()
        {
            AmmoSwapEvent?.Invoke();
        }

        public void EventCallPickUpAmmo(string ammoName, int quantity)
        {
            PickUpAmmoEvent?.Invoke(ammoName, quantity);
        }

        public void EventCallPlayerHpIncrease(float hp)
        {
            PlayerHpIncreaseEvent?.Invoke(hp);
        }

        public void EventCallPlayerHpDeduction(float hp)
        {
            PlayerHpDeductionEvent?.Invoke(hp);
        }
    }
}
