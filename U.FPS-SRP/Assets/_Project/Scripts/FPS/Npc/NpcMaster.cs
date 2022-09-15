/*
 * NpcMaster -
 * Created by : Allan N. Murillo
 * Last Edited : 4/7/2020
 */

using UnityEngine;

namespace ANM.FPS.Npc
{
    public class NpcMaster : MonoBehaviour
    {
        [Header("Animation Parameter Names")] public string boolIsPursuing = "isPursuing"; //  animation bool
        public string triggerGetHit = "GetHit"; //  animation trigger
        public string triggerAttack = "Attack"; //  animation trigger
        public string triggerRecovered = "Recovered"; //  animation trigger
    
        public delegate void GeneralEventHandler();
        public delegate void StatsEventHandler(int stat);
        public delegate void NpcRelationsChangeEventHandler();

        public event GeneralEventHandler EventNpcDie; //  npc has died
        public event GeneralEventHandler EventNpcIdle; //  npc is idle
        public event GeneralEventHandler EventNpcWalk; //  npc is walking
        public event GeneralEventHandler EventNpcAttack; //  npc is ready for an attack
        public event GeneralEventHandler EventNpcGetHit; //  npc has been stunned from the critical hit
        public event GeneralEventHandler EventNpcRecovered; //  npc has recovered from stun
        public event GeneralEventHandler EventNpcCriticalHealth; //  npc has low health
        public event GeneralEventHandler EventNpcHealthRecovered; //  npc has healed past critical lvls

        public event StatsEventHandler EventNpcHeal; //  npc has healed its health
        public event StatsEventHandler EventNpcTakeDmg; //  npc has lost some health

        public event NpcRelationsChangeEventHandler EventNpcRelationsChange; //  npc has changed faction relation status
    
    
        public void CallEventNpcDie()
        {
            // Debug.Log("NPCMaster::EventNPCDie() received!");
            EventNpcDie?.Invoke();
        }

        public void CallEventNpcLowHealth()
        {
            // Debug.Log("NPCMaster::EventNpcCriticalHealth() received!");
            EventNpcCriticalHealth?.Invoke();
        }

        public void CallEventNpcHealthRecovered()
        {
            // Debug.Log("NPCMaster::EventNpcHealthRecovered() received!");
            EventNpcHealthRecovered?.Invoke();
        }

        public void CallEventNpcWalk()
        {
            // Debug.Log("NPCMaster::EventNpcWalk() received!");
            EventNpcWalk?.Invoke();
        }

        public void CallEventNpcGetHit()
        {
            // Debug.Log("NPCMaster::EventNpcGetHit() received!");
            EventNpcGetHit?.Invoke();
        }

        public void CallEventNpcAttack()
        {
            // Debug.Log("NPCMaster::EventNpcAttack() received!");
            EventNpcAttack?.Invoke();
        }

        public void CallEventNpcRecovered()
        {
            // Debug.Log("NPCMaster::EventNpcRecovered() received!");
            EventNpcRecovered?.Invoke();
        }

        public void CallEventNpcIdle()
        {
            // Debug.Log("NPCMaster::EventNpcIdle() received!");
            EventNpcIdle?.Invoke();
        }

        //    TODO : Use only 1 method to damage NPC
        public void CallEventNpcDeductHealth(int hp)
        {
            // Debug.Log("NPCMaster::EventNpcTakeDmg() received : " + hp);
            EventNpcTakeDmg?.Invoke(hp);
        }

        public void CallEventNpcIncreaseHealth(int hp)
        {
            // Debug.Log("NPCMaster::EventNpcHeal() received!");
            EventNpcHeal?.Invoke(hp);
        }

        public void CallEventNpcRelationsChange()
        {
            // Debug.Log("NPCMaster::EventNpcRelationsChange() received!");
            EventNpcRelationsChange?.Invoke();
        }
    }
}
