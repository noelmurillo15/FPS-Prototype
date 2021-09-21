/*
 * GunApplyDamage -
 * Created by : Allan N. Murillo
 * Last Edited : 4/10/2020
 */

using UnityEngine;

namespace ANM.FPS.Items.Guns
{
    public class GunApplyDamage : MonoBehaviour
    {
        [SerializeField] private int baseDamage = 10;
        private GunMaster _gunMaster;


        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            if (_gunMaster.playerEquipped || _gunMaster.npcEquipped)
            {
                _gunMaster.EventShotEnemy -= ApplyDamage;
            }
        }

        private void Initialize()
        {
            _gunMaster = GetComponent<GunMaster>();
            if (!_gunMaster.playerEquipped && !_gunMaster.npcEquipped)
            {
                // Debug.Log("    - GunApplyDamage is being disabled");
                enabled = false;
                return;
            }
            _gunMaster.EventShotEnemy += ApplyDamage;
        }

        public int GetAttackDamage()
        {
            return baseDamage;
        }

        private void ApplyDamage(RaycastHit hit, Transform hitTransform)
        {
            // Debug.Log("GunApplyDamage::ApplyDamage()");
            hitTransform.SendMessage("ProcessDamage", baseDamage,
                SendMessageOptions.DontRequireReceiver); //  NpcTakeDamage.cs
            hitTransform.SendMessage("CallEventPlayerHpDeduction", baseDamage,
                SendMessageOptions.DontRequireReceiver); //  PlayerMaster.cs
            hitTransform.SendMessage("SetMyAttacker", transform.root,
                SendMessageOptions.DontRequireReceiver); //  NpcStatePattern.cs
        }
    }
}
