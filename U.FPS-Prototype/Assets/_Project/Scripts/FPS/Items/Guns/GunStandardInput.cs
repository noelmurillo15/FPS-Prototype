/*
 * GunStandardInput -
 * Created by : Allan N. Murillo
 * Last Edited : 4/10/2020
 */

using UnityEngine;
using System.Collections;
using ANM.FPS.Player.Input;

namespace ANM.FPS.Items.Guns
{
    public class GunStandardInput : MonoBehaviour
    {
        [SerializeField] private bool isAutomatic = false;
        [SerializeField] private bool hasBurstFire = false;
        [SerializeField] private bool isBurstFireActive = false;
        [SerializeField] private float attackRate = 0.5f;
        [SerializeField] private float burstRate = 0.1f;

        private float _nextAttack;
        private float _burstBuffer;
        private float _reloadBuffer;
        private GunMaster _gunMaster;


        private void OnEnable()
        {
            if (!transform.root.CompareTag("Player"))
            {
                enabled = false;
                return;
            }

            InitializeForPlayer();
        }

        private void OnDisable()
        {
            ThrowGunAway();
        }

        public float GetAttackRate()
        {
            return attackRate;
        }

        private void InitializeForPlayer()
        {
            Debug.Log("GunStandardInput::InitializeForPlayer()");
            _gunMaster = GetComponent<GunMaster>();
            _gunMaster.isGunLoaded = true;
            transform.root.GetComponent<FpsController>().AssignGunInput(this, isAutomatic);
        }

        private void AttemptAttack()
        {
            _nextAttack = Time.time + attackRate;

            if (_gunMaster.isGunLoaded)
            {
                _gunMaster.CallEventPlayerInput();
            }
            else
            {
                _gunMaster.CallEventGunUnusable();
            }
        }

        private IEnumerator RunBurstFire()
        {
            AttemptAttack();
            yield return new WaitForSeconds(burstRate);
            AttemptAttack();
            yield return new WaitForSeconds(burstRate);
            AttemptAttack();
            yield return new WaitForEndOfFrame();
        }

        public void ThrowGunAway()
        {
            if (!transform.root.CompareTag("Player")) return;
            transform.root.GetComponent<FpsController>().AssignGunInput();
        }

        #region Detect Input

        public void FireInput()
        {
            //    Burst or non-auto
            if (!(Time.time > _nextAttack)) return;

            if (!isBurstFireActive)
            {
                AttemptAttack();
            }
            else
            {
                StartCoroutine(RunBurstFire());
            }
        }

        public void ReloadInput()
        {
            if (!(Time.time > _reloadBuffer)) return;
            _reloadBuffer = Time.time + attackRate;
            _gunMaster.CallEventReload();
        }

        public void BurstFireToggleInput()
        {
            if (!hasBurstFire) return;
            if (!(Time.time > _burstBuffer)) return;
            _burstBuffer = Time.time + attackRate;
            isBurstFireActive = !isBurstFireActive;
            _gunMaster.CallEventToggleBurst();
        }

        #endregion
    }
}
