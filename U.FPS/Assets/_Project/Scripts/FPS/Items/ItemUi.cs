/*
 * ItemUI -
 * Created by : Allan N. Murillo
 * Last Edited : 4/11/2020
 */

using UnityEngine;

namespace ANM.FPS.Items
{
    public class ItemUi : MonoBehaviour
    {
        [SerializeField] private GameObject myUi;
        private ItemMaster _itemMaster;
        

        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            _itemMaster.EventObjectPickup -= EnableUi;
            _itemMaster.EventObjectThrow -= DisableUi;
        }
        
        private void Initialize()
        {
            _itemMaster = GetComponent<ItemMaster>();
            _itemMaster.EventObjectPickup += EnableUi;
            _itemMaster.EventObjectThrow += DisableUi;
        }

        private void EnableUi()
        {
            myUi.SetActive(true);
        }

        private void DisableUi()
        {
            myUi.SetActive(false);
        }
    }
}
