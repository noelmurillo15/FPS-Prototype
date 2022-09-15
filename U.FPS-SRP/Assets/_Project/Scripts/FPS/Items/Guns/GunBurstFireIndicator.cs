/*
 * GunBurstFireIndicator -
 * Created by : Allan N. Murillo
 * Last Edited : 4/10/2020
 */

using UnityEngine;

namespace ANM.FPS.Items.Guns
{
    public class GunBurstFireIndicator : MonoBehaviour
    {
        [SerializeField] private GameObject burstFireIndicator = null;
        private GunMaster _gunMaster;


        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            if(_gunMaster.playerEquipped) _gunMaster.EventToggleBurst -= ToggleIndicator;
        }

        private void Initialize()
        {
            _gunMaster = GetComponent<GunMaster>();
            if (!_gunMaster.playerEquipped && !_gunMaster.npcEquipped)
            {
                // Debug.Log("    - GunBurstFireIndicator is being disabled");
                enabled = false;
                return;
            }
            if(_gunMaster.playerEquipped) _gunMaster.EventToggleBurst += ToggleIndicator;
        }

        private void ToggleIndicator()
        {
            Debug.Log("GunBurstFireIndicator::ToggleIndicator()");
            if (burstFireIndicator != null)
            {
                burstFireIndicator.SetActive(!burstFireIndicator.activeSelf);
            }
        }
    }
}
