/*
 * GunReset -
 * Created by : Allan N. Murillo
 * Last Edited : 4/10/2020
 */

using UnityEngine;

namespace ANM.FPS.Items.Guns
{
    public class GunReset : MonoBehaviour
    {
        private GunMaster _gunMaster;
        private ItemMaster _itemMaster;


        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            if (_itemMaster != null)
            {
                _itemMaster.EventObjectThrow -= ResetGun;
            }
        }

        private void Initialize()
        {
            _gunMaster = GetComponent<GunMaster>();
            if (!_gunMaster.playerEquipped && !_gunMaster.npcEquipped)
            {
                // Debug.Log("    - GunReset is being disabled");
                enabled = false;
                return;
            }

            if (GetComponent<ItemMaster>() == null) return;
            _itemMaster = GetComponent<ItemMaster>();
            _itemMaster.EventObjectThrow += ResetGun;
        }

        private void ResetGun()
        {
            Debug.Log("GunReset::ResetGun()");
            _gunMaster.CallEventGunReset();
        }
    }
}
