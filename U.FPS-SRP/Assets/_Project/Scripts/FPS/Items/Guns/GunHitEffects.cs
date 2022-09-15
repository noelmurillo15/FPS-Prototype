/*
 * GunHitEffects -
 * Created by : Allan N. Murillo
 * Last Edited : 4/10/2020
 */

using UnityEngine;

namespace ANM.FPS.Items.Guns
{
    public class GunHitEffects : MonoBehaviour
    {
        [SerializeField] private GameObject defaultHitEffect = null;
        [SerializeField] private GameObject enemyHitEffect = null;
        private GunMaster _gunMaster;


        private void OnEnable()
        {
            Initialize();
            // _gunMaster.EventShotDefault += SpawnDefaultHitEffect;
            
        }

        private void OnDisable()
        {
            // _gunMaster.EventShotDefault -= SpawnDefaultHitEffect;
            if(_gunMaster.playerEquipped) _gunMaster.EventShotEnemy -= SpawnEnemyHitEffect;
        }

        private void Initialize()
        {
            _gunMaster = GetComponent<GunMaster>();
            if (!_gunMaster.playerEquipped && !_gunMaster.npcEquipped)
            {
                // Debug.Log("    - GunHitEffects is being disabled");
                enabled = false;
                return;
            }
            if(_gunMaster.playerEquipped) _gunMaster.EventShotEnemy += SpawnEnemyHitEffect;
        }

        private void SpawnDefaultHitEffect(RaycastHit hit, Transform t)
        {
            Debug.Log("GunHitEffects::SpawnDefaultHitEffect()");
            if (defaultHitEffect != null)
            {
                Instantiate(defaultHitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }

        private void SpawnEnemyHitEffect(RaycastHit hit, Transform t)
        {
            Debug.Log("GunHitEffects::SpawnEnemyHitEffect()");
            if (enemyHitEffect != null)
            {
                Instantiate(enemyHitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }
}
