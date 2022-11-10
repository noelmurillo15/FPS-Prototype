/*
 * NpcInvestigateState -
 * Created by : Allan N. Murillo
 * Last Edited : 4/9/2020
 */

using UnityEngine;

namespace ANM.FPS.Npc.States
{
    public class NpcInvestigateState : INpcState
    {
        private readonly NpcStatePattern _npc;
        private Vector3 _lookAtTarget;
        private RaycastHit _hit;


        public NpcInvestigateState(NpcStatePattern npcStatePattern) => _npc = npcStatePattern;

        #region INpc Methods

        public void UpdateState()
        {
            Investigate();
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

        public void ToMeleeAttackState()
        {
            _npc.ActivateMelee();
        }

        public void ToRangeAttackState()
        {
            _npc.ActivateRanged();
        }

        #endregion

        #region Investigate

        private void Investigate()
        {
            if (_npc.AttackTarget == null)
            {
                ToPatrolState();
                return;
            }

            _lookAtTarget = _npc.AttackTarget.position;
            if (!Physics.Linecast(_npc.head.position, _lookAtTarget, out _hit, _npc.sightLayers))
            {
                ToAlertState();
                return;
            }

            _npc.AttackTarget = _hit.transform.root;
            // Debug.DrawLine(_npc.head.position, _lookAtTarget, _npc.debugColor, _npc.checkRate);
            if (_npc.hasMeleeAttack && _hit.distance <= _npc.meleeAttackRange)
            {
                ToMeleeAttackState();
                return;
            }

            if (_npc.hasGun && _hit.distance <= _npc.gunAttackRange)
            {
                ToRangeAttackState();
                return;
            }

            Pursue();
        }

        private void Pursue()
        {
            _npc.AttackTarget = null;
            if (!(_hit.distance < _npc.sightRange)) return;
            //Debug.Log("NPC is heading towards suspicious location");
            _npc.MoveTowardsNoSample(_hit.transform.root.position, 1f, 10f);
            ToChaseState();
        }

        #endregion
    }
}
