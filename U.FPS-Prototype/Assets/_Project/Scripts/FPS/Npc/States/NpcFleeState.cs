/*
 * NpcFleeState -
 * Created by : Allan N. Murillo
 * Last Edited : 4/9/2020
 */

using UnityEngine;

namespace ANM.FPS.Npc.States
{
    public class NpcFleeState : INpcState
    {
        private readonly NpcStatePattern _npc;
        private Vector3 _dirToEnemy;


        public NpcFleeState(NpcStatePattern npcStatePattern) => _npc = npcStatePattern;

        #region INpc Methods

        //    TODO : Fix Flee
        public void UpdateState()
        {
            ToPatrolState();
            //CheckShouldFlee();
        }

        public void ToPatrolState()
        {
            _npc.isFleeing = false;
            _npc.ActivatePatrol();
        }

        public void ToAlertState(){}

        public void ToChaseState(){}

        public void ToMeleeAttackState(){}

        public void ToRangeAttackState(){}

        #endregion

        #region Flee

        /// <summary>
        /// Npc will move in the direction away from the enemy
        /// If NPC had no Attack Target, move in the direction facing away from your current rotation
        /// Continue to flee until NPC detects no enemies within flee range
        /// </summary>
        private void CheckShouldFlee()
        {
            if (_npc.IsPlayingAttackAnimation) return;

            if (!_npc.isFleeing)
            {
                Vector3 checkPos;
                if (_npc.AttackTarget != null)
                {
                    var position = _npc.root.position;
                    _dirToEnemy = position - _npc.AttackTarget.position;
                    checkPos = position + _dirToEnemy * _npc.sightRange;
                    _npc.AttackTarget = null;
                    if (!_npc.canMove || !_npc.MoveToOverride(checkPos)) return;
                    Debug.DrawLine(_npc.root.position, checkPos, Color.gray, _npc.checkRate);
                    _npc.isFleeing = true;
                }
                else
                {
                    checkPos = _npc.root.position + (-_npc.root.forward * _npc.sightRange);
                    if (!_npc.canMove || !_npc.MoveToOverride(checkPos)) return;
                    Debug.DrawLine(_npc.root.position, checkPos, Color.gray, _npc.checkRate);
                    _npc.isFleeing = true;
                }
            }
            else
            {
                if (_npc.myNavMeshAgent.isStopped)
                {
                    ToPatrolState();
                    return;
                }

                if (_npc.enemyCollidersInRange.Length == 0) ToPatrolState();
            }
        }

        #endregion
    }
}
