/*
 * NpcFollowState -
 * Created by : Allan N. Murillo
 * Last Edited : 4/9/2020
 */

using UnityEngine;

namespace ANM.FPS.Npc.States
{
    public class NpcFollowState : INpcState
    {
        private readonly NpcStatePattern _npc;


        public NpcFollowState(NpcStatePattern npcStatePattern) => _npc = npcStatePattern;

        #region INpc Methods

        public void UpdateState()
        {
            FollowTarget();
        }

        public void ToPatrolState()
        {
            _npc.currentState = _npc.patrolState;
        }

        public void ToAlertState(){}

        public void ToChaseState(){}

        public void ToMeleeAttackState(){}

        public void ToRangeAttackState(){}

        #endregion

        #region Follow

        private void FollowTarget()
        {
            if (_npc.followTarget == null)
            {
                ToPatrolState();
                return;
            }

            if (_npc.canMove)
            {
                if (Vector3.Distance(_npc.root.position, _npc.followTarget.position) > _npc.detectBehindRange)
                {
                    _npc.MoveTo(_npc.followTarget.position);
                }
            }

            ToPatrolState();
        }

        #endregion
    }
}
