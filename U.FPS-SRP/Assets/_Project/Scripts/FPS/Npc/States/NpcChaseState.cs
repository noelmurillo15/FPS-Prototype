/*
 * NpcChaseState -
 * Created by : Allan N. Murillo
 * Last Edited : 4/9/2020
 */

using UnityEngine;

namespace ANM.FPS.Npc.States
{
    public class NpcChaseState : INpcState
    {
        private readonly NpcStatePattern _npc;
        private float _capturedDistance;


        public NpcChaseState(NpcStatePattern npcStatePattern) => _npc = npcStatePattern;

        #region INpc Methods

        public void UpdateState()
        {
            SelectClosestTarget();
        }

        public void ToPatrolState()
        {
            _npc.ActivatePatrol();
        }

        public void ToAlertState()
        {
            _npc.ActivateAlert();
        }

        public void ToMeleeAttackState()
        {
            _npc.ActivateMelee();
        }

        public void ToRangeAttackState()
        {
            _npc.ActivateRanged();
        }

        public void ToChaseState(){}

        #endregion

        #region Chase

        private void SelectClosestTarget()
        {
            if (_npc.enemyCollidersInRange.Length == 0)
            {
                ToPatrolState();
                return;
            }

            _npc.AttackTarget = null;
            _capturedDistance = _npc.sightRange * 1.2f;
            foreach (var enemy in _npc.enemyCollidersInRange)
            {
                if (enemy.transform.root.CompareTag("Untagged")) continue;
                var distanceToTarget = Vector3.Distance(_npc.transform.position, enemy.transform.position);
                if (!(distanceToTarget < _capturedDistance)) continue;
                if (!VisibilityCalculations(enemy.transform.position)) continue;
                _capturedDistance = distanceToTarget;
                _npc.AttackTarget = enemy.transform.root;
            }
            
            // Debug.DrawLine(_npc.head.position, _npc.AttackTarget.position, _npc.debugColor, _npc.checkRate);
            if (Mathf.Approximately(_capturedDistance, _npc.sightRange * 1.2f))
            {
                ToAlertState();
                return;
            }
            
            if (_npc.hasMeleeAttack && _capturedDistance <= _npc.meleeAttackRange)
            {
                ToMeleeAttackState();
                return;
            }

            if (_npc.hasGun && _capturedDistance <= _npc.gunAttackRange)
            {
                ToRangeAttackState();
                return;
            }

            if (_npc.AttackTarget != null)
            {
                _npc.MoveTowardsNoSample(_npc.AttackTarget.root.position, 1f, 5f);
            }
            else
            {
                ToPatrolState();
            }
        }

        private bool VisibilityCalculations(Vector3 target)
        {
            var lookAtPoint = target;
            var heading = lookAtPoint - _npc.root.position;
            var dotProduct = Vector3.Dot(heading, _npc.head.forward);
            return dotProduct > 5;
        }

        #endregion
    }
}
