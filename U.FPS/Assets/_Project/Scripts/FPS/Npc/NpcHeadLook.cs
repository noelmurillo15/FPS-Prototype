/*
 * NpcHeadLook -
 * Created by : Allan N. Murillo
 * Last Edited : 4/7/2020
 */

using UnityEngine;

namespace ANM.FPS.Npc
{
    public class NpcHeadLook : MonoBehaviour
    {
        private Animator _animator;
        private NpcStatePattern _npc;


        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _npc = GetComponent<NpcStatePattern>();
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Head Look towards Attack Target
        /// </summary>
        private void OnAnimatorIK(int layerIndex)
        {
            if (!_animator.enabled)
            {
                Debug.Log("NPC animator is disabled!");
                return;
            }
            
            if (_npc.AttackTarget != null)
            {
                var root = _npc.AttackTarget.root;
                // Debug.Log(_npc.name + " should be looking at ATTACK Target : " + root.name);
                _animator.SetLookAtWeight(1f, 0.25f, 1f, 0f, .25f);
                var position = root.position;
                var lookAtPosition = new Vector3(position.x, position.y + 1.2f, position.z);
                _animator.SetLookAtPosition(lookAtPosition);
            }
            else
            {
                // Debug.Log(_npc.name + " has No Target");
                _animator.SetLookAtWeight(0f);
            }
        }
    }
}
