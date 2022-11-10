/*
 * ItemTag -
 * Created by : Allan N. Murillo
 * Last Edited : 4/10/2020
 */

using UnityEngine;

namespace ANM.FPS.Items
{
    public class ItemTag : MonoBehaviour
    {
        [SerializeField] private string itemTag;


        private void Start()
        {
            SetTag();
        }

        private void SetTag()
        {
            if (itemTag == "") itemTag = "Untagged";
            transform.tag = itemTag;
        }
    }
}
