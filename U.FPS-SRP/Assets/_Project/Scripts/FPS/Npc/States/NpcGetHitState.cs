/*
 * NpcGetHitState -
 * Created by : Allan N. Murillo
 * Last Edited : 4/9/2020
 */

using UnityEngine;

namespace ANM.FPS.Npc.States
{
    public class NpcGetHitState : INpcState
    {
        private readonly NpcStatePattern _npc;
        private const float InformRate = 10f;
        private float _nextInform;
        private Collider[] _allies;


        public NpcGetHitState(NpcStatePattern npcStatePattern)
        {
            _nextInform = Random.Range(0f, 10f);
            _npc = npcStatePattern;
        }

        #region INpc Methods

        public void UpdateState()
        {
            InformAllies();
        }

        public void ToPatrolState(){ }

        public void ToAlertState(){ }

        public void ToChaseState(){ }

        public void ToMeleeAttackState(){ }

        public void ToRangeAttackState(){ }

        #endregion

        #region Get Hit
        
        private void InformAllies()
        {
            if (!(Time.time > _nextInform)) return;
            _nextInform = Time.time + InformRate;
            SetMyselfToInvestigate();

            if (_npc.AttackTarget == null) return;
            if (CheckAttackerDistance()) AlertAllies();
        }

        private void AlertAllies()
        {
            _allies = Physics.OverlapSphere(_npc.root.position, _npc.detectBehindRange * 3f, _npc.friendlyLayers);
            foreach (var nearbyAlly in _allies)
            {
                if (nearbyAlly.transform.root == _npc.root) continue;
                if (nearbyAlly.transform.root.GetComponent<NpcStatePattern>() == null) continue;
                var ally = nearbyAlly.transform.root.GetComponent<NpcStatePattern>();
                if (ally.currentState != ally.patrolState && ally.currentState != ally.followState) continue;
                ally.ActivateInvestigate(_npc.AttackTarget);
                break;
            }
        }

        private void SetMyselfToInvestigate()
        {
            if (!_npc.hasBeenHit)
            {
                Debug.Log(_npc.name + " is in getHitState but was not critically hit- going to captured state");
                _npc.ActivateStoredState();
                return;
            }

            if (_npc.capturedState != _npc.patrolState) return;
            // Debug.Log(_npc.name + " was critically hit- CAPTURED STATE SET TO INVESTIGATE ANYTHING ELSE WILL OVERRIDE!");
            _npc.StoreState(_npc.investigateRate, _npc.investigateState, Color.cyan);
        }

        private bool CheckAttackerDistance()
        {
            return Vector3.Distance(_npc.root.position, _npc.AttackTarget.position) <= _npc.sightRange * 1.25f;
        }

        #endregion
    }
}
