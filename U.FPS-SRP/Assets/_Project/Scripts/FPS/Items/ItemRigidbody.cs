/*
 * ItemRigidbody -
 * Created by : Allan N. Murillo
 * Last Edited : 4/11/2020
 */

using UnityEngine;

namespace ANM.FPS.Items
{
    [RequireComponent(typeof(Rigidbody))]
    public class ItemRigidbody : MonoBehaviour
    {
        [SerializeField] private Rigidbody[] rigidbodies = null;
        private ItemMaster _itemMaster;


        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            _itemMaster.EventObjectThrow -= SetKinematicFalse;
            _itemMaster.EventObjectPickup -= SetKinematicTrue;
            _itemMaster.EventObjectThrow -= SetGravityTrue;
            _itemMaster.EventObjectPickup -= SetGravityFalse;
        }

        private void Initialize()
        {
            _itemMaster = GetComponent<ItemMaster>();
            _itemMaster.EventObjectThrow += SetKinematicFalse;
            _itemMaster.EventObjectPickup += SetKinematicTrue;

            _itemMaster.EventObjectThrow += SetGravityTrue;
            _itemMaster.EventObjectPickup += SetGravityFalse;
            CheckIfInInventory();
        }

        private void CheckIfInInventory()
        {
            if (transform.root.CompareTag("Player"))
            {
                SetKinematicTrue();
            }
        }

        private void SetGravityTrue()
        {
            foreach (var rb in rigidbodies)
            {
                rb.useGravity = true;
            }
        }

        private void SetGravityFalse()
        {
            foreach (var rb in rigidbodies)
            {
                rb.useGravity = false;
            }
        }

        private void SetKinematicTrue()
        {
            foreach (var rb in rigidbodies)
            {
                rb.isKinematic = true;
            }
        }

        private void SetKinematicFalse()
        {
            foreach (var rb in rigidbodies)
            {
                rb.isKinematic = false;
            }
        }
    }
}
