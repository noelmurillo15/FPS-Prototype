/*
 * NpcAnimations -
 * Created by : Allan N. Murillo
 * Last Edited : 4/7/2020
 */

using UnityEngine;

namespace ANM.FPS.Npc
{
    [RequireComponent(typeof(Animator))]
    public class NpcAnimations : MonoBehaviour
    {
        private Animator _animator;
        private NpcMaster _npcMaster;


        private void Initialize()
        {
            _animator = GetComponent<Animator>();
            _npcMaster = GetComponent<NpcMaster>();
        }

        private void OnEnable()
        {
            Initialize();
            _npcMaster.EventNpcAttack += ActivateAttackAnimation;
            _npcMaster.EventNpcWalk += ActivateWalkingAnimation;
            _npcMaster.EventNpcIdle += ActivateIdleAnimation;
            _npcMaster.EventNpcRecovered += ActivateRecoveredAnimation;
            _npcMaster.EventNpcGetHit += ActivateGetHitAnimation;
        }

        private void OnDisable()
        {
            _npcMaster.EventNpcAttack -= ActivateAttackAnimation;
            _npcMaster.EventNpcWalk -= ActivateWalkingAnimation;
            _npcMaster.EventNpcIdle -= ActivateIdleAnimation;
            _npcMaster.EventNpcRecovered -= ActivateRecoveredAnimation;
            _npcMaster.EventNpcGetHit -= ActivateGetHitAnimation;
        }

        #region Event Animations

        private void ActivateWalkingAnimation()
        {
            if (_animator.enabled) _animator.SetBool(_npcMaster.boolIsPursuing, true);
        }

        private void ActivateIdleAnimation()
        {
            if (_animator.enabled) _animator.SetBool(_npcMaster.boolIsPursuing, false);
        }

        private void ActivateAttackAnimation()
        {
            if (_animator.enabled) _animator.SetTrigger(_npcMaster.triggerAttack);
        }

        private void ActivateRecoveredAnimation()
        {
            if (_animator.enabled) _animator.SetTrigger(_npcMaster.triggerRecovered);
        }

        private void ActivateGetHitAnimation()
        {
            if (_animator.enabled) _animator.SetTrigger(_npcMaster.triggerGetHit);
        }

        #endregion
    }
}
