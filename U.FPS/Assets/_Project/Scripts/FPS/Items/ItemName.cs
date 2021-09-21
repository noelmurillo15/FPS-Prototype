/*
 * ItemName -
 * Created by : Allan N. Murillo
 * Last Edited : 4/11/2020
 */

using UnityEngine;

namespace ANM.FPS.Items
{
    public class ItemName : MonoBehaviour
    {
        [SerializeField] private string itemName;


        private void Start()
        {
            SetName();
        }

        private void SetName()
        {
            if (itemName == "") itemName = "Unknown";
            transform.name = itemName;
        }
    }
}
