/*
 * ItemSetPosition -
 * Created by : Allan N. Murillo
 * Last Edited : 4/10/2020
 */

using UnityEngine;

namespace ANM.FPS.Items
{
    public class ItemSetPosition : MonoBehaviour
    {
        [SerializeField] private Vector3 itemLocalPosition = Vector3.zero;


        private void OnEnable()
        {
            SetPositionOnCharacter();
            // itemMaster.EventObjectPickup += SetPositionOnPlayer;
        }

        private void OnDisable()
        {
            // itemMaster.EventObjectPickup -= SetPositionOnPlayer;
        }

        private void SetPositionOnCharacter()
        {
            if (transform.root.CompareTag("Player") || 
                transform.root.CompareTag("Npc") || transform.root.CompareTag("Enemy"))
            {
                transform.localPosition = itemLocalPosition;
            }
        }
    }
}
