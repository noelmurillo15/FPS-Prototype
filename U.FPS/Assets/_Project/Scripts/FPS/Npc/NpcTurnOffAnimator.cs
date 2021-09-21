/*
 * NpcTurnOffAnimator -
 * Created by : Allan N. Murillo
 * Last Edited : 4/7/2020
 */

using UnityEngine;

namespace ANM.FPS.Npc
{
    [RequireComponent(typeof(Animator))]
    public class NpcTurnOffAnimator : MonoBehaviour
    {
        private Animator _animator;
        private NpcMaster _npcMaster;


        private void Initialize()
        {
            _npcMaster = GetComponent<NpcMaster>();
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            Initialize();
            _npcMaster.EventNpcDie += TurnOffAnimator;
        }

        private void OnDisable()
        {
            _npcMaster.EventNpcDie -= TurnOffAnimator;
        }

        private void TurnOffAnimator()
        {
            _animator.enabled = false;
        }
    }
}
