/*
 * NpcTakeDamage -
 * Created by : Allan N. Murillo
 * Last Edited : 4/7/2020
 */

using UnityEngine;

namespace ANM.FPS.Npc
{
    public class NpcTakeDamage : MonoBehaviour
    {
        [SerializeField] private int damageMultiplier = 1;
        [SerializeField] private bool shouldRemoveCollider;
        private NpcMaster _npcMaster;


        private void Initialize()
        {
            _npcMaster = transform.root.GetComponent<NpcMaster>();
            gameObject.layer = LayerMask.NameToLayer("Damageable");
        }

        private void OnEnable()
        {
            Initialize();
            _npcMaster.EventNpcDie += RemoveThis;
        }

        private void OnDisable()
        {
            _npcMaster.EventNpcDie -= RemoveThis;
        }

        public void ProcessDamage(int dmg)
        {    //    TODO : Called from a send message, pls change this
            var dmgToApply = dmg * damageMultiplier;
            //Debug.Log("ProcessDamage Msg has been received and calculated : " + dmgToApply);
            _npcMaster.CallEventNpcDeductHealth(dmgToApply);
        }

        private void RemoveThis()
        {
            gameObject.layer = LayerMask.NameToLayer("Ragdoll");
            if (shouldRemoveCollider)
            {
                if (GetComponent<Collider>() != null) Destroy(GetComponent<Collider>());
                if (GetComponent<Rigidbody>() != null) Destroy(GetComponent<Rigidbody>());
            }
            Destroy(this);
        }
    }
}
