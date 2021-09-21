/*
 * GunMuzzleFlash -
 * Created by : Allan N. Murillo
 * Last Edited : 4/10/2020
 */

using UnityEngine;

namespace ANM.FPS.Items.Guns
{
    public class GunMuzzleFlash : MonoBehaviour
    {
        [SerializeField] private ParticleSystem muzzleFlash = null;
        private GunMaster _gunMaster;


        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            if(_gunMaster.playerEquipped) _gunMaster.EventPlayerInput -= PlayMuzzleFlash;
            else if(_gunMaster.npcEquipped) _gunMaster.EventNpcInput -= PlayMuzzleFlashNpc;
        }

        private void Initialize()
        {
            _gunMaster = GetComponent<GunMaster>();
            if (!_gunMaster.playerEquipped && !_gunMaster.npcEquipped)
            {
                // Debug.Log("    - GunMuzzleFlash is being disabled");
                enabled = false;
                return;
            }
            if(_gunMaster.playerEquipped) _gunMaster.EventPlayerInput += PlayMuzzleFlash;
            else _gunMaster.EventNpcInput += PlayMuzzleFlashNpc;
        }

        private void PlayMuzzleFlash()
        {
            Debug.Log("GunMuzzleFlash::PlayMuzzleFlash()");
            if (muzzleFlash != null)
            {
                muzzleFlash.Play();
            }
        }

        private void PlayMuzzleFlashNpc(float dummy, Vector3 dummy2)
        {
            // Debug.Log("GunMuzzleFlash::PlayMuzzleFlashNpc()");
            if (muzzleFlash != null)
            {
                muzzleFlash.Play();
            }
        }
    }
}
