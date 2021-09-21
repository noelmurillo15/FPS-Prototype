/*
 * AmmoBox -
 * Created by : Allan N. Murillo
 * Last Edited : 3/24/2020
 */

using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace ANM.FPS.Player
{
    public class AmmoBox : MonoBehaviour
    {
        private PlayerMaster _playerMaster;
        public List<AmmoTypes> typesOfAmmo;

        public TMP_Text uiCurrentCount;
        public TMP_Text uiMaxCount;
        public TMP_Text uiGunName;

        [System.Serializable]
        public class AmmoTypes
        {
            public string ammoName;
            public int ammoMaxCount;
            public int currentCount;

            public AmmoTypes(string name, int maxAmount, int currAmount)
            {
                ammoName = name;
                ammoMaxCount = maxAmount;
                currentCount = currAmount;
            }
        }

        private void Initialize()
        {
            _playerMaster = GetComponent<PlayerMaster>();
        }

        private void OnEnable()
        {
            Initialize();
            _playerMaster.PickUpAmmoEvent += PickedUpAmmo;
        }

        private void OnDisable()
        {
            _playerMaster.PickUpAmmoEvent -= PickedUpAmmo;
        }

        private void PickedUpAmmo(string ammoName, int amount)
        {
            foreach (var ammo in typesOfAmmo.Where(ammo => ammo.ammoName == ammoName))
            {
                ammo.currentCount += amount;

                if (ammo.currentCount > ammo.ammoMaxCount)
                {
                    ammo.currentCount = ammo.ammoMaxCount;
                }
                _playerMaster.EventCallAmmoSwap();
            }
        }
    }
}