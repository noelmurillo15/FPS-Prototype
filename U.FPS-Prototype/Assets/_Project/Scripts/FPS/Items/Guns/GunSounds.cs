/*
 * GunSounds -
 * Created by : Allan N. Murillo
 * Last Edited : 4/10/2020
 */

using UnityEngine;

namespace ANM.FPS.Items.Guns
{
    public class GunSounds : MonoBehaviour
    {
        [SerializeField] private float shootVolume = .4f;
        [SerializeField] private float reloadVolume = .5f;
        [SerializeField] private AudioClip[] shootSounds = null;
        [SerializeField] private AudioClip reloadSound = null;

        //  Cached Variables
        private GunMaster _gunMaster;
        private Transform _myTransform;


        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            if (!_gunMaster.playerEquipped) return;
            _gunMaster.EventPlayerInput -= PlayShootSound;
            _gunMaster.EventNpcInput -= PlayNpcShootSound;
        }

        private void Initialize()
        {
            _myTransform = transform;
            _gunMaster = GetComponent<GunMaster>();
            if (!_gunMaster.playerEquipped && !_gunMaster.npcEquipped)
            {
                // Debug.Log("    - GunSounds is being disabled");
                enabled = false;
                return;
            }

            if (!_gunMaster.playerEquipped) return;
            _gunMaster.EventPlayerInput += PlayShootSound;
            _gunMaster.EventNpcInput += PlayNpcShootSound;
        }

        private void PlayShootSound()
        {
            Debug.Log("GunSounds::PlayShootSound()");
            if (shootSounds.Length <= 0) return;
            int index = Random.Range(0, shootSounds.Length);
            AudioSource.PlayClipAtPoint(shootSounds[index], _myTransform.position, shootVolume);
        }

        private void PlayNpcShootSound(float dummy, Vector3 dummy2)
        {
            Debug.Log("GunSounds::PlayNpcShootSound()");
            PlayShootSound();
        }

        // Called by the reload Animation event
        public void PlayReloadSound()
        {
            Debug.Log("GunSounds::PlayReloadSound()");
            if (reloadSound != null)
            {
                AudioSource.PlayClipAtPoint(reloadSound, _myTransform.position, reloadVolume);
            }
        }
    }
}
