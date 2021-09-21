/*
 * NpcMeleeAttackState -
 * Created by : Allan N. Murillo
 * Last Edited : 4/9/2020
 */

using UnityEngine;

namespace ANM.FPS.Npc.States
{
    public class NpcMeleeAttackState : INpcState
    {
        private readonly NpcStatePattern _npc;
        private float _meleeAttackRate;


        public NpcMeleeAttackState(NpcStatePattern npcStatePattern) => _npc = npcStatePattern;

        #region INpc Methods

        public void UpdateState()
        {
            Look();
            TryAttack();
        }

        public void ToPatrolState()
        {
            _npc.ActivatePatrol();
        }

        public void ToAlertState()
        {
            _npc.ActivateAlert();
        }

        public void ToChaseState()
        {
            _npc.ActivateChase();
        }

        public void ToMeleeAttackState(){}

        public void ToRangeAttackState(){}

        #endregion

        #region MeleeState
        
        private void Look()
        {
            if (_npc.AttackTarget == null || !_npc.hasMeleeAttack)
            {
                ToPatrolState();
                return;
            }

            if (_npc.IsPlayingAttackAnimation) return;

            var colliders = Physics.OverlapSphere(_npc.root.position, _npc.meleeAttackRange, _npc.enemyLayers);
            if (colliders.Length == 0)
            {
                //Debug.Log(_npc.name + " found no hostile enemies within melee range");
                ToChaseState();
                return;
            }

            for (var i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].transform.root != _npc.AttackTarget) continue;
                TurnTowardsTarget();
                return;
            }
        }

        /// <summary>
        /// Will call the Melee Attack event if Npc is close enough to the target &
        /// Do nothing while currently attacking
        /// </summary>
        private void TryAttack()
        {
            if (_npc.currentState != _npc.meleeAttackState) return;

            if (!_npc.IsPlayingAttackAnimation && _npc.AttackTarget != null)
            {
                if (Time.time > _meleeAttackRate)
                {
                    if (Vector3.Distance(_npc.AttackTarget.position, _npc.root.position) <=
                        _npc.meleeAttackRange)
                    {
                        _meleeAttackRate = Time.time + _npc.meleeAttackRate;
                        var position = _npc.AttackTarget.position;
                        var newPos = new Vector3(position.x, _npc.root.position.y, position.z);
                        _npc.root.LookAt(newPos);
                        _npc.GetMaster().CallEventNpcAttack();
                        _npc.IsPlayingAttackAnimation = true;
                    }
                    else
                    {
                        ToChaseState();
                    }
                }
            }
        }

        private void TurnTowardsTarget()
        {
            var position = _npc.AttackTarget.position;
            var newPos = new Vector3(position.x, _npc.root.position.y, position.z);
            _npc.transform.LookAt(newPos);
        }

        #endregion
    }
}
