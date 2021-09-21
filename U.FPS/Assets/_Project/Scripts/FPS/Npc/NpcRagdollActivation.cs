/*
 * NpcRagdollActivation -
 * Created by : Allan N. Murillo
 * Last Edited : 4/7/2020
 */

using UnityEngine;

namespace ANM.FPS.Npc
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class NpcRagdollActivation : MonoBehaviour
    {
        private NpcMaster _npcMaster;
        private Rigidbody _rigidbody;
        private Collider _collider;


        private void Initialize()
        {
            _npcMaster = transform.root.GetComponent<NpcMaster>();
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        private void OnEnable()
        {
            Initialize();
            //    TODO :  play animation, then ragdoll
            _npcMaster.EventNpcDie += ActivateRagdoll;
        }

        private void OnDisable()
        {
            _npcMaster.EventNpcDie -= ActivateRagdoll;
        }

        private void ActivateRagdoll()
        {
            if (_collider != null)
            {
                _collider.enabled = true;
                _collider.isTrigger = false;
            }

            if (_rigidbody != null)
            {
                _rigidbody.isKinematic = false;
                _rigidbody.useGravity = true;
            }

            GameObject go;
            (go = gameObject).layer = LayerMask.NameToLayer("Default");
            go.tag = "Untagged";
        }
    }
}
