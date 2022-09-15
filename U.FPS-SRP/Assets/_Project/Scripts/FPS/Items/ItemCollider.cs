/*
 * ItemCollider -
 * Created by : Allan N. Murillo
 * Last Edited : 4/11/2020
 */

using UnityEngine;

namespace ANM.FPS.Items
{
    [RequireComponent(typeof(Collider))]
    public class ItemCollider : MonoBehaviour
    {
        [SerializeField] private Collider[] colliders = null;
        [SerializeField] private PhysicMaterial myPhysicsMat = null;
        private ItemMaster _itemMaster;


        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            _itemMaster.EventObjectThrow -= EnableColliders;
            _itemMaster.EventObjectPickup -= DisableColliders;
        }

        private void Initialize()
        {
            _itemMaster = GetComponent<ItemMaster>();
            _itemMaster.EventObjectThrow += EnableColliders;
            _itemMaster.EventObjectPickup += DisableColliders;
            CheckIfInInventory();
        }

        private void CheckIfInInventory()
        {
            if (transform.root.CompareTag("Player"))
            {
                DisableColliders();
            }
        }

        private void EnableColliders()
        {
            foreach (var t in colliders)
            {
                t.enabled = true;
                if (myPhysicsMat != null)
                {
                    t.material = myPhysicsMat;
                }
            }
        }

        private void DisableColliders()
        {
            foreach (var t in colliders)
            {
                t.enabled = false;
            }
        }
    }
}
