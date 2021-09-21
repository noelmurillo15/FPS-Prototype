/*
 * NpcTurnOffStatePattern -
 * Created by : Allan N. Murillo
 * Last Edited : 4/7/2020
 */

using UnityEngine;

namespace ANM.FPS.Npc
{
    [RequireComponent(typeof(NpcStatePattern))]
    public class NpcTurnOffStatePattern : MonoBehaviour
    {
        private NpcMaster _npcMaster;
        private NpcStatePattern _statePattern;


        private void Initialize()
        {
            _npcMaster = GetComponent<NpcMaster>();
            _statePattern = GetComponent<NpcStatePattern>();
        }

        private void OnEnable()
        {
            Initialize();
            _npcMaster.EventNpcDie += TurnOffStatePattern;
        }

        private void OnDisable()
        {
            _npcMaster.EventNpcDie -= TurnOffStatePattern;
        }

        private void TurnOffStatePattern()
        {
            _statePattern.enabled = false;
        }
    }
}
