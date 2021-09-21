/*
 * NpcRangedAttackState -
 * Created by : Allan N. Murillo
 * Last Edited : 4/9/2020
 */

using System.Linq;
using ANM.FPS.Items.Guns;
using UnityEngine;

namespace ANM.FPS.Npc.States
{
    public class NpcRangedAttackState : INpcState
    {
        private readonly NpcStatePattern _npc;
        private Vector3 _lookAtTarget;
        private float _attackCooldown;
        private bool _toggle;

        
        public NpcRangedAttackState(NpcStatePattern npcStatePattern)
        {
            _npc = npcStatePattern;
            _npc.lookAt = Vector3.zero;
        }

        #region INpc Methods

        public void UpdateState()
        {
            Look();
        }

        public void ToPatrolState()
        {
            _npc.lookAt = Vector3.zero;
            _npc.ActivatePatrol();
        }

        public void ToAlertState()
        {
            _npc.lookAt = Vector3.zero;
            _npc.ActivateAlert();
        }

        public void ToChaseState()
        {
            _npc.lookAt = Vector3.zero;
            _npc.ActivateChase();
        }

        public void ToMeleeAttackState()
        {
            _npc.lookAt = Vector3.zero;
            _npc.ActivateMelee();
        }

        public void ToRangeAttackState(){}

        #endregion

        #region Ranged Attack

        private void Look()
        {
            if (!(Time.time > _attackCooldown)) return;
            _attackCooldown = Time.time + _npc.gunAttackRate;

            if (_npc.AttackTarget == null || TargetTooFar())
            {
                ToChaseState();
                return;
            }

            var position = _npc.AttackTarget.root.position;
            var lookAtStart = _npc.head.position + _npc.root.forward;
            _lookAtTarget = new Vector3(position.x, position.y + _npc.offset, position.z);

            //    Damageable or terrain layer
            var damageMask = 1 << 16 | 1 << 14;
            if (!Physics.Linecast(lookAtStart, _lookAtTarget, out var hit, damageMask))
            {
                //Debug.DrawLine(lookAtStart, _lookAtTarget, Color.blue, _npc.checkRate);
                _npc.AttackTarget = null;
                ToChaseState();
                return;
            }

            if (_npc.enemyTags.Any(tag => hit.transform.root.CompareTag(tag)))
            {
                // Debug.Log("NPC is attempting ranged attack");
                _lookAtTarget = hit.transform.position;
                _npc.AttackTarget = hit.transform;
                _npc.lookAt = _lookAtTarget;
                WeaponFire();
                return;
            }

            if (_npc.friendlyTags.Any(tag => hit.transform.root.CompareTag(tag)))
            {
                if (!_npc.canMove) return;
                // Debug.Log(hit.transform.root == _npc.root
                //     ? "NPC hit himself during ranged attack line cast"
                //     : "NPC is attempting to pivot because of an ally in LOS");
                PivotPosition();
                return;
            }
            // Debug.Log("NPC has terrain in his LOS while ranged");
            ToPatrolState();
        }

        private void PivotPosition()
        {
            _toggle = !_toggle;
            Vector3 directionToPivot;
            if (_toggle) directionToPivot = _npc.root.position + _npc.root.right;
            else directionToPivot = _npc.root.position + _npc.root.right * -1f;
            _npc.canMove = false;
            _npc.MoveTowardsNoSample(directionToPivot, 0.5f, 5);
        }

        private bool TargetTooClose()
        {
            var distanceToTarget = Vector3.Distance(_npc.lookAt, _npc.root.position);
            return distanceToTarget <= _npc.meleeAttackRange;
        }

        private bool TargetTooFar()
        {
            var distanceToTarget = Vector3.Distance(_npc.AttackTarget.position, _npc.root.position);
            return distanceToTarget > _npc.gunAttackRange;
        }

        /// <summary>
        /// Ranged Attack Spread of 0.1 will always hit target, rarely any head shots
        /// Ranged Attack Spread of 0.5 = 88% hit chance, 16% head shots
        /// Ranged Attack Spread of 1 = 57% hit chance, 22% head shots
        /// Ranged Attack Spread of 2 = 19% hit chance, 17% head shots
        /// </summary>
        private void WeaponFire()
        {
            _npc.gun?.GetComponent<GunMaster>().CallEventNpcInput(_npc.rangedAttackSpread, _lookAtTarget);
            if (TargetTooClose()) ToMeleeAttackState();
            else ToChaseState();
        }
        #endregion
    }
}
