/*
 * ItemAmmo - attach to an item that can be picked up and give ammo
 * Created by : Allan N. Murillo
 * Last Edited : 4/10/2020
 */

using ANM.FPS.Player;
using UnityEngine;

namespace ANM.FPS.Items.Consumable
{
    public class ItemAmmo : MonoBehaviour
    {
        public int quantity;
        public string ammoName;
        public bool isTriggerPickup;
        private ItemMaster _itemMaster;


        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            _itemMaster.EventObjectPickup -= TakeAmmo;
        }

        private void Initialize()
        {
            _itemMaster = GetComponent<ItemMaster>();
            _itemMaster.EventObjectPickup += TakeAmmo;

            if (!isTriggerPickup) return;
            if (GetComponent<Collider>() != null)
                GetComponent<Collider>().isTrigger = isTriggerPickup;

            if (GetComponent<Rigidbody>() != null)
                GetComponent<Rigidbody>().isKinematic = isTriggerPickup;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && isTriggerPickup)
            {
                TakeAmmo();
            }
        }

        private void TakeAmmo()
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMaster>()
                .EventCallPickUpAmmo(ammoName, quantity);
            Destroy(gameObject);
        }
    }
}
