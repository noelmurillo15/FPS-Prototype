/*
 * GunAmmoUi - 
 * Created by : Allan N. Murillo
 * Last Edited : 4/10/2020
 */

using ANM.FPS.Player;
using TMPro;
using UnityEngine;

namespace ANM.FPS.Items.Guns
{
    public class GunAmmoUi : MonoBehaviour
    {
        [SerializeField] private TMP_Text currentAmmoField;
        [SerializeField] private TMP_Text carriedAmmoField;
        [SerializeField] private TMP_Text gunName;
        private GunMaster _gunMaster;


        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            currentAmmoField = null;
            carriedAmmoField = null;
            if (_gunMaster.playerEquipped) _gunMaster.EventAmmoChanged -= UpdateAmmoUi;
        }

        private void Initialize()
        {
            _gunMaster = GetComponent<GunMaster>();
            if (!_gunMaster.playerEquipped && !_gunMaster.npcEquipped)
            {
                // Debug.Log("    - GunAmmoUi is being disabled");
                enabled = false;
                return;
            }
            if (_gunMaster.playerEquipped) _gunMaster.EventAmmoChanged += UpdateAmmoUi;
        }

        private void UpdateAmmoUi(int currentAmmo, int carriedAmmo)
        {
            Debug.Log("GunAmmoUi::UpdateAmmoUi()");
            if (!transform.root.CompareTag("Player")) return;
            if (currentAmmoField == null)
            {
                var ammoBox = transform.root.GetComponent<AmmoBox>();
                currentAmmoField = ammoBox.uiCurrentCount;
                carriedAmmoField = ammoBox.uiMaxCount;
                gunName = ammoBox.uiGunName;
                gunName.text = this.gameObject.name;
            }

            if (currentAmmoField != null)
            {
                currentAmmoField.text = currentAmmo.ToString();
            }

            if (carriedAmmoField != null)
            {
                carriedAmmoField.text = carriedAmmo.ToString();
            }
        }
    }
}
