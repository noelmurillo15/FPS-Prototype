/*
 * NpcAlertState -
 * Created by : Allan N. Murillo
 * Last Edited : 4/9/2020
 */

using System.Linq;
using UnityEngine;

namespace ANM.FPS.Npc.States
{
    public class NpcAlertState : INpcState
    {
        private readonly NpcStatePattern _npc;
        private const float InformRate = 5f;
        private float _nextInform;
        private bool _reset;

        private Collider[] _allies;
        private Transform _spine;
        private Transform _head;
        private Transform _lHand;
        private Transform _rHand;
        private Transform _rLeg;
        private Transform _lLeg;
        private Animator _targetAnimator;

        private Vector3 _lookAtTarget;
        private RaycastHit _headHit;
        private RaycastHit _spineHit;
        private RaycastHit _lHandHit;
        private RaycastHit _rHandHit;
        private RaycastHit _lLegHit;
        private RaycastHit _rLegHit;
        private RaycastHit _confirmedHit;

        private int _detectionCount;
        private int _lastDetectionCount;

        private int _hitCount;

        public NpcAlertState(NpcStatePattern npcStatePattern)
        {
            _npc = npcStatePattern;
            Reset();
        }

        #region INpc Methods

        public void UpdateState()
        {
            Look();
        }

        public void ToPatrolState()
        {
            Reset();
            _npc.ActivatePatrol();
        }

        public void ToChaseState(){}

        public void ToAlertState(){}

        public void ToMeleeAttackState(){}

        public void ToRangeAttackState(){}

        #endregion

        #region Alert

        private void Look()
        {
            if (_npc.AttackTarget == null)
            {
                ToPatrolState();
                return;
            }

            _hitCount = 0;
            _lastDetectionCount = _detectionCount;

            if (_reset)
            {
                _reset = false;
                _targetAnimator = _npc.AttackTarget.root.GetComponent<Animator>();
                _head = _targetAnimator.GetBoneTransform(HumanBodyBones.Head);
                _spine = _targetAnimator.GetBoneTransform(HumanBodyBones.Spine);
                _lLeg = _targetAnimator.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
                _rLeg = _targetAnimator.GetBoneTransform(HumanBodyBones.RightLowerLeg);
                _lHand = _targetAnimator.GetBoneTransform(HumanBodyBones.LeftThumbProximal);
                _rHand = _targetAnimator.GetBoneTransform(HumanBodyBones.RightThumbProximal);
            }

            var myHead = _npc.head.position;
            if (Physics.Linecast(myHead, _lLeg.position, out _lLegHit, _npc.sightLayers))
            {
                if (_npc.enemyTags.Any(tag => _lLegHit.transform.root.CompareTag(tag)))
                {
                    _hitCount++;
                    _confirmedHit = _lLegHit;
                    Debug.DrawLine(myHead, _lLeg.position, _npc.GetDebugColor(), _npc.checkRate);
                }
            }

            if (Physics.Linecast(myHead, _rLeg.position, out _rLegHit, _npc.sightLayers))
            {
                if (_npc.enemyTags.Any(tag => _rLegHit.transform.root.CompareTag(tag)))
                {
                    _hitCount++;
                    _confirmedHit = _rLegHit;
                    Debug.DrawLine(myHead, _rLeg.position, _npc.GetDebugColor(), _npc.checkRate);
                }
            }

            if (Physics.Linecast(myHead, _rHand.position, out _rHandHit, _npc.sightLayers))
            {
                if (_npc.enemyTags.Any(tag => _rHandHit.transform.root.CompareTag(tag)))
                {
                    _hitCount++;
                    _confirmedHit = _rHandHit;
                    Debug.DrawLine(myHead, _rHand.position, _npc.GetDebugColor(), _npc.checkRate);
                }
            }

            if (Physics.Linecast(myHead, _lHand.position, out _lHandHit, _npc.sightLayers))
            {
                if (_npc.enemyTags.Any(tag => _lHandHit.transform.root.CompareTag(tag)))
                {
                    _hitCount++;
                    _confirmedHit = _lHandHit;
                    Debug.DrawLine(myHead, _lHand.position, _npc.GetDebugColor(), _npc.checkRate);
                }
            }

            if (Physics.Linecast(myHead, _head.position, out _headHit, _npc.sightLayers))
            {
                if (_npc.enemyTags.Any(tag => _headHit.transform.root.CompareTag(tag)))
                {
                    _hitCount++;
                    _confirmedHit = _headHit;
                    Debug.DrawLine(myHead, _head.position, _npc.GetDebugColor(), _npc.checkRate);
                }
            }

            if (Physics.Linecast(myHead, _spine.position, out _spineHit, _npc.sightLayers))
            {
                if (_npc.enemyTags.Any(tag => _spineHit.transform.root.CompareTag(tag)))
                {
                    _hitCount++;
                    _confirmedHit = _spineHit;
                    Debug.DrawLine(myHead, _spine.position, _npc.GetDebugColor(), _npc.checkRate);
                }
            }

            if (_hitCount > 1)
            {
                _detectionCount += _hitCount;
                if (_confirmedHit.distance <= _npc.detectBehindRange || _detectionCount >= _npc.requiredDetectionCount)
                {
                    InvestigateSensedTarget(_confirmedHit.transform);
                    return;
                }
            }

            if (_detectionCount != _lastDetectionCount) return;
            //Debug.Log("NPC failed alert detection test - Sight Layer");
            _npc.AttackTarget = null;
            ToPatrolState();
        }

        private void Reset()
        {
            _reset = true;
            _detectionCount = 0;
            _lastDetectionCount = 0;
        }

        private void InvestigateSensedTarget(Transform target)
        {
            Reset();
            InformNearbyAllies(target);
            _npc.ActivateInvestigate(target);
        }

        private void InformNearbyAllies(Transform target)
        {
            if (!(Time.time > _nextInform)) return;
            _nextInform = Time.time + InformRate;
            
            _allies = Physics.OverlapSphere(_npc.root.position, _npc.detectBehindRange * 2f, _npc.friendlyLayers);
            foreach (var nearbyAlly in _allies)
            {
                if (nearbyAlly.transform.root == _npc.root) continue;
                var ally = nearbyAlly.transform.GetComponentInParent<NpcStatePattern>();
                if (ally == null || ally.currentState != ally.patrolState && 
                    ally.currentState != ally.followState) continue;
                ally.ActivateInvestigate(target);
            }
        }

        #endregion
    }
}
