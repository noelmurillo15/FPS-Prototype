/*
 * ItemPickup -
 * Created by : Allan N. Murillo
 * Last Edited : 4/11/2020
 */

using ANM.FPS.Items.Guns;
using ANM.FPS.Player;
using UnityEngine;

namespace ANM.FPS.Items
{
    public class ItemPickup : MonoBehaviour
    {
        private ItemMaster _itemMaster;


        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            _itemMaster.EventPickupAction -= PickupAction;
        }

        private void Initialize()
        {
            _itemMaster = GetComponent<ItemMaster>();
            _itemMaster.EventPickupAction += PickupAction;
        }

        private void PickupAction(Transform parent)
        {
            transform.SetParent(parent);
            _itemMaster.CallEventObjectPickup();
            transform.gameObject.SetActive(false);
            if (!transform.root.CompareTag("Player")) return;
            transform.SetParent(transform.root.GetComponent<PlayerMaster>().playerCam.transform);
            GetComponent<GunStandardInput>().enabled = true;
        }
    }
}
