/*
 * ItemSetLayer -
 * Created by : Allan N. Murillo
 * Last Edited : 4/11/2020
 */

using UnityEngine;

namespace ANM.FPS.Items
{
    public class ItemSetLayer : MonoBehaviour
    {
        [SerializeField] private string throwLayer;
        [SerializeField] private string pickupLayer;
        private ItemMaster _itemMaster;


        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            _itemMaster.EventObjectThrow -= ItemThrow;
            _itemMaster.EventObjectPickup -= ItemPickup;
        }

        private void Initialize()
        {
            _itemMaster = GetComponent<ItemMaster>();
            _itemMaster.EventObjectThrow += ItemThrow;
            _itemMaster.EventObjectPickup += ItemPickup;
            SetLayerOnEnable();
        }

        private void ItemPickup()
        {
            SetLayer(transform, pickupLayer);
        }

        private void ItemThrow()
        {
            SetLayer(transform, throwLayer);
        }

        private void SetLayerOnEnable()
        {
            if (pickupLayer == "") pickupLayer = "Default";
            if (throwLayer == "") throwLayer = "Item";
            
            if (transform.root.CompareTag("Player")) ItemPickup();
            else ItemThrow();
        }

        private static void SetLayer(Transform myTrans, string itemLayerName)
        {
            myTrans.gameObject.layer = LayerMask.NameToLayer(itemLayerName);
            for (int i = 0; i < myTrans.childCount; i++) SetLayer(myTrans.GetChild(i), itemLayerName);
        }
    }
}
