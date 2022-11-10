/*
 * NpcPatrolState -
 * Created by : Allan N. Murillo
 * Last Edited : 4/9/2020
 */

using UnityEngine;
using UnityEngine.AI;

namespace ANM.FPS.Npc.States
{
    public class NpcPatrolState : INpcState
    {
        private readonly NpcStatePattern _npc;
        private const float WanderRate = 10f;
        private float _nextWanderTarget;
        private float _dotProduct;
        private int _nextWaypoint;
        private Vector3 _heading;
        private Vector3 _lookAtPoint;


        public NpcPatrolState(NpcStatePattern npcStatePattern) => _npc = npcStatePattern;

        #region INpc Methods

        public void UpdateState()
        {
            Look();
            Patrol();
        }

        public void ToAlertState()
        {
            _npc.ActivateAlert();
        }

        public void ToChaseState(){}

        public void ToPatrolState(){}

        public void ToMeleeAttackState(){}

        public void ToRangeAttackState(){}

        #endregion

        #region Patrol Behaviour

        private void Look()
        {
            if (_npc.enemyCollidersInRange.Length == 0) return;
            // Debug.Log("Patrol Scan Found " + _npc.enemyCollidersInRange.Length + " in range");

            foreach (var other in _npc.enemyCollidersInRange)
            {
                if (other.transform.root.CompareTag("Untagged")) continue;
                
                VisibilityCalculations(other.transform.position);

                if (!Physics.Linecast(_npc.head.position, _lookAtPoint, out var hit, _npc.sightLayers))
                {
                    //Debug.Log("NPC failed a line test during Patrol - Sight Layer");
                    continue;
                }

                if (hit.distance > _npc.sightRange) continue;
                //Debug.DrawLine(_npc.head.position, _lookAtPoint, _npc.debugColor, _npc.checkRate);
                foreach (var enemyTag in _npc.enemyTags)
                {
                    if (!hit.transform.root.CompareTag(enemyTag)) continue;
                    if (!(hit.distance < _npc.detectBehindRange) && !(_dotProduct > 5)) continue;
                    OnAlert(hit.transform.root);
                    return;
                }
            }
        }

        private void VisibilityCalculations(Vector3 target)
        {
            _lookAtPoint = target;
            _heading = _lookAtPoint - _npc.root.position;
            _dotProduct = Vector3.Dot(_heading, _npc.head.forward);
        }

        private void OnAlert(Transform target)
        {
            _npc.AttackTarget = target;
            _npc.MoveTowardsNoSample(target.root.position, 1f, 10f);
            ToAlertState();
        }

        private void Patrol()
        {
            if (_npc.currentState != _npc.patrolState) return; // needed if this function can overwrite npc.currentState

            if (!_npc.myNavMeshAgent.enabled) return; //  If npc is unable to move

            if (_npc.followTarget != null)
            {
                _npc.currentState = _npc.followState;
                return;
            }

            if (!(Time.time > _nextWanderTarget)) return;
            if (!_npc.canMove) return;
            if (_npc.waypoints.Length > 0)
            {
                _nextWanderTarget = Time.time + WanderRate;
                _nextWaypoint = Random.Range(0, _npc.waypoints.Length);
                _npc.MoveTo(_npc.waypoints[_nextWaypoint].position);
            }
            else
            {
                if (!RandomWanderTarget(_npc.root.position, _npc.gunAttackRange,
                    out _npc.wanderDestination)) return;
                _nextWanderTarget = Time.time + WanderRate;
                _npc.MoveToNoSample(_npc.wanderDestination);
            }
        }

        private bool RandomWanderTarget(Vector3 center, float range, out Vector3 result)
        {
            var randomPoint = center + Random.insideUnitSphere * range;
            randomPoint.y = _npc.root.position.y + _npc.myNavMeshAgent.height;

            if (NavMesh.SamplePosition(randomPoint, out var navHit, range, NavMesh.AllAreas))
            {
                result = navHit.position;
                return true;
            }

            result = center;
            return false;
        }

        #endregion
    }
}
