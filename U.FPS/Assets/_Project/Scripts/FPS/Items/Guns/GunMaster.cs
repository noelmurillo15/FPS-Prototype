/*
 * GunMaster -
 * Created by : Allan N. Murillo
 * Last Edited : 4/11/2020
 */

using UnityEngine;

namespace ANM.FPS.Items.Guns
{
    public class GunMaster : MonoBehaviour
    {
        public bool npcEquipped;
        public bool playerEquipped;
    
        public bool isGunLoaded;
        public bool isReloading;

        public delegate void GeneralEventHandler();
        public event GeneralEventHandler EventPlayerInput;
        public event GeneralEventHandler EventGunUnusable;
        public event GeneralEventHandler EventReload;
        public event GeneralEventHandler EventGunReset;
        public event GeneralEventHandler EventToggleBurst;

        public delegate void GunHitEventHandler(RaycastHit hitPosition, Transform hitTransform);
        public event GunHitEventHandler EventShotDefault;
        public event GunHitEventHandler EventShotEnemy;

        public delegate void GunAmmoEventHandler(int currentAmmo, int totalAmmo);
        public event GunAmmoEventHandler EventAmmoChanged;

        public delegate void GunCrosshairEventHandler(float speed);
        public event GunCrosshairEventHandler EventSpeedCaptured;

        public delegate void GunNpcEventHandler(float attackSpread, Vector3 target);
        public event GunNpcEventHandler EventNpcInput;


        private void Awake()
        {
            npcEquipped = false;
            playerEquipped = false;
            if (transform.root.CompareTag("Player")) playerEquipped = true;
            else if (transform.root.CompareTag("Enemy")) npcEquipped = true;
            else if (transform.root.CompareTag("Npc")) npcEquipped = true;
        }

        public void CallEventPlayerInput()
        {
            EventPlayerInput?.Invoke();
        }

        public void CallEventGunUnusable()
        {
            EventGunUnusable?.Invoke();
        }

        public void CallEventReload()
        {
            EventReload?.Invoke();
        }

        public void CallEventGunReset()
        {
            EventGunReset?.Invoke();
        }

        public void CallEventToggleBurst()
        {
            EventToggleBurst?.Invoke();
        }

        public void CallEventShotDefault(RaycastHit hitPosition, Transform hitTransform)
        {
            EventShotDefault?.Invoke(hitPosition, hitTransform);
        }

        public void CallEventShoot(RaycastHit hitPosition, Transform hitTransform)
        {
            EventShotEnemy?.Invoke(hitPosition, hitTransform);
        }

        public void CallEventAmmoChanged(int currentAmmo, int totalAmmo)
        {
            EventAmmoChanged?.Invoke(currentAmmo, totalAmmo);
        }

        public void CallEventSpeedCaptured(float speed)
        {
            EventSpeedCaptured?.Invoke(speed);
        }

        //  Called by NPCRangedAttackState
        public void CallEventNpcInput(float attackSpread, Vector3 target)
        {
            EventNpcInput?.Invoke(attackSpread, target);
        }
    }
}
