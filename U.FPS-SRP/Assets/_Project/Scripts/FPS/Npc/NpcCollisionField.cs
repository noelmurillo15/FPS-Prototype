/*
 * NpcCollisionField -
 * Created by : Allan N. Murillo
 * Last Edited : 4/7/2020
 */

using UnityEngine;

namespace ANM.FPS.Npc
{
    [RequireComponent(typeof(Rigidbody))]
    public class NpcCollisionField : MonoBehaviour
    {
        private NpcMaster _npcMaster;
        private Rigidbody _rigidbody;

        private int _damageToApply;
        private const float DamageFactor = 0.05f;

        public float massRequirement = 60;
        public float speedRequirement = 8;


        private void Initialize()
        {
            _npcMaster = transform.root.GetComponent<NpcMaster>();
        }

        private void OnEnable()
        {
            Initialize();
            _npcMaster.EventNpcDie += DisableThis;
        }

        private void OnDisable()
        {
            _npcMaster.EventNpcDie -= DisableThis;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Rigidbody>() == null) return;
            //  TIP : checking for sqrMagnitude is faster than checking magnitude
            var rb = other.GetComponent<Rigidbody>();
            if (!(rb.mass >= massRequirement) ||
                !(rb.velocity.sqrMagnitude >= speedRequirement * speedRequirement)) return;
            _damageToApply = (int) (rb.mass * rb.velocity.magnitude * DamageFactor);
            Debug.Log("Collision Impact caused : " + _damageToApply + " damage");
            _npcMaster.CallEventNpcDeductHealth(_damageToApply);
        }

        private void DisableThis()
        {
            gameObject.SetActive(false);
        }
    }
}
