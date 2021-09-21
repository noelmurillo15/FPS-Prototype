/*
 * ItemIcon -
 * Created by : Allan N. Murillo
 * Last Edited : 4/11/2020
 */

using UnityEngine;

namespace ANM.FPS.Items
{
    [System.Serializable]
    public class ItemIcon : MonoBehaviour
    {
        [SerializeField] private Sprite image;
        [SerializeField] private string toolTip;


        public Sprite Image
        {
            get => image;
            set => image = value;
        }

        public string ToolTip
        {
            get => toolTip;
            set => toolTip = value;
        }
    }
}