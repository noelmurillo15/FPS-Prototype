/*
 * GunAmmo -
 * Created by : Allan N. Murillo
 * Last Edited : 4/10/2020
 */

using FPS;
using UnityEngine;
using System.Collections;
using ANM.FPS.Player;

namespace ANM.FPS.Items.Guns
{
    public class GunAmmo : MonoBehaviour
    {
        [SerializeField] private int clipSize = 0;
        [SerializeField] private int currentAmmo = 0;
        [SerializeField] private string myAmmoName = string.Empty;
        [SerializeField] private float reloadTime = 0f;

        private static readonly int Reload = Animator.StringToHash("Reload");
        private PlayerMaster _playerMaster;
        private GunMaster _gunMaster;
        private Animator _animator;
        private AmmoBox _ammoBox;


        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            if (_playerMaster == null) return;
            _gunMaster.EventReload -= TryToReload;
            _gunMaster.EventPlayerInput -= DeductAmmo;
            _gunMaster.EventGunUnusable -= TryToReload;
            _gunMaster.EventGunReset -= ResetGunReloading;
            _gunMaster.EventPlayerInput -= CheckAmmoStatus;
            _playerMaster.AmmoSwapEvent -= UiAmmoUpdate;
        }

        private void Initialize()
        {
            _gunMaster = GetComponent<GunMaster>();
            if (!_gunMaster.playerEquipped && !_gunMaster.npcEquipped)
            {
                // Debug.Log("    - GunAmmo is being disabled");
                enabled = false;
                return;
            }

            if (!_gunMaster.playerEquipped) return;
            SanityCheck();
            CheckAmmoStatus();
            _animator = GetComponent<Animator>();
            _gunMaster.EventReload += TryToReload;
            _gunMaster.EventPlayerInput += DeductAmmo;
            _gunMaster.EventGunUnusable += TryToReload;
            _gunMaster.EventGunReset += ResetGunReloading;
            _gunMaster.EventPlayerInput += CheckAmmoStatus;
            _ammoBox = GameReferences.myPlayer.GetComponent<AmmoBox>();
            _playerMaster = GameReferences.myPlayer.GetComponent<PlayerMaster>();
            _playerMaster.AmmoSwapEvent += UiAmmoUpdate;
            if (_ammoBox != null) StartCoroutine(UpdateAmmoUiWhenEnabling());
            if (_gunMaster.isReloading) ResetGunReloading();
            Debug.Log("    - Gun Ammo was found equipped on Player");
        }

        private void DeductAmmo()
        {
            Debug.Log("GunAmmo::DeductAmmo()");
            currentAmmo--;
            UiAmmoUpdate();
        }

        private void TryToReload()
        {
            Debug.Log("GunAmmo::TryToReload()");
            for (int i = 0; i < _ammoBox.typesOfAmmo.Count; i++)
            {
                if (_ammoBox.typesOfAmmo[i].ammoName != myAmmoName) continue;
                if (currentAmmo != clipSize && !_gunMaster.isReloading && _ammoBox.typesOfAmmo[i].currentCount > 0)
                {
                    _gunMaster.isReloading = true;
                    _gunMaster.isGunLoaded = false;

                    if (_animator != null)
                    {
                        _animator.SetTrigger(Reload);
                    }
                    else
                    {
                        StartCoroutine(ReloadWithoutAnimation());
                    }
                }

                break;
            }
        }

        private void CheckAmmoStatus()
        {
            Debug.Log("GunAmmo::CheckAmmoStatus()");
            if (currentAmmo <= 0)
            {
                currentAmmo = 0;
                _gunMaster.isGunLoaded = false;
            }
            else if (currentAmmo > 0)
            {
                _gunMaster.isGunLoaded = true;
            }
        }

        private void SanityCheck()
        {
            Debug.Log("GunAmmo::SanityCheck()");
            if (currentAmmo > clipSize)
            {
                currentAmmo = clipSize;
            }
        }

        private void UiAmmoUpdate()
        {
            Debug.Log("GunAmmo::UiAmmoUpdate()");
            for (int i = 0; i < _ammoBox.typesOfAmmo.Count; i++)
            {
                if (_ammoBox.typesOfAmmo[i].ammoName != myAmmoName) continue;
                _gunMaster.CallEventAmmoChanged(currentAmmo, _ammoBox.typesOfAmmo[i].currentCount);
                break;
            }
        }

        private void ResetGunReloading()
        {
            Debug.Log("GunAmmo::ResetGunReloading()");
            _gunMaster.isReloading = false;
            CheckAmmoStatus();
            UiAmmoUpdate();
        }

        /// <summary>
        /// Called by Animation - Gun_Reload
        /// Attempt to add ammo to current
        /// </summary>
        public void OnReloadComplete()
        {
            Debug.Log("GunAmmo::OnReloadComplete()");
            for (int i = 0; i < _ammoBox.typesOfAmmo[i].currentCount; i++)
            {
                if (_ammoBox.typesOfAmmo[i].ammoName != myAmmoName) continue;
                int ammoTopUp = clipSize - currentAmmo;

                if (_ammoBox.typesOfAmmo[i].currentCount >= ammoTopUp)
                {
                    currentAmmo += ammoTopUp;
                    _ammoBox.typesOfAmmo[i].currentCount -= ammoTopUp;
                }

                else if (_ammoBox.typesOfAmmo[i].currentCount < ammoTopUp &&
                         _ammoBox.typesOfAmmo[i].currentCount != 0)
                {
                    currentAmmo += _ammoBox.typesOfAmmo[i].currentCount;
                    _ammoBox.typesOfAmmo[i].currentCount = 0;
                }

                break;
            }

            ResetGunReloading();
        }

        private IEnumerator ReloadWithoutAnimation()
        {
            yield return new WaitForSeconds(reloadTime);
            OnReloadComplete();
        }

        private IEnumerator UpdateAmmoUiWhenEnabling()
        {
            yield return
                new WaitForSeconds(
                    0.05f); //  this is a fudge factor to ensure that the ui is updated when changing weapons
            UiAmmoUpdate();
        }
    }
}
