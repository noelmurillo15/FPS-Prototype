/*
 * NpcTurnOffNavAgent -
 * Created by : Allan N. Murillo
 * Last Edited : 4/7/2020
 */

using UnityEngine;
using UnityEngine.AI;

namespace ANM.FPS.Npc
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NpcTurnOffNavAgent : MonoBehaviour
    {
        private NpcMaster _npcMaster;
        private NavMeshAgent _navAgent;


        private void Initialize()
        {
            _npcMaster = GetComponent<NpcMaster>();
            _navAgent = GetComponent<NavMeshAgent>();
        }

        private void OnEnable()
        {
            Initialize();
            _npcMaster.EventNpcDie += TurnOffNavMeshAgent;
        }

        private void OnDisable()
        {
            _npcMaster.EventNpcDie -= TurnOffNavMeshAgent;
        }

        private void TurnOffNavMeshAgent()
        {
            _navAgent.enabled = false;
        }
    }
}
