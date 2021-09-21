/*
 * NpcHealth -
 * Created by : Allan N. Murillo
 * Last Edited : 4/7/2020
 */

using UnityEngine;

namespace ANM.FPS.Npc
{
    public class NpcHealth : MonoBehaviour
    {
        private NpcMaster _npcMaster;
        public int npcHp = 100;
        public int npcMaxHp = 100;
        private bool _criticalHit;


        private void Initialize()
        {
            _npcMaster = GetComponent<NpcMaster>();
        }

        private void OnEnable()
        {
            Initialize();
            _npcMaster.EventNpcTakeDmg += DeductHp;
            _npcMaster.EventNpcHeal += IncreaseHp;
        }

        private void OnDisable()
        {
            _npcMaster.EventNpcTakeDmg -= DeductHp;
            _npcMaster.EventNpcHeal -= IncreaseHp;
        }

        private void IncreaseHp(int hp)
        {
            npcHp += hp;
            if (npcHp > npcMaxHp)
            {
                npcHp = npcMaxHp;
            }

            CheckHealthFraction();
        }

        private void DeductHp(int hp)
        {
            npcHp -= hp;
            if (npcHp <= 0)
            {
                npcHp = 0;
                _npcMaster.CallEventNpcDie();
                Destroy(gameObject, Random.Range(2f, 5f));
                return;
            }

            CheckHealthFraction();
        }

        private void CheckHealthFraction()
        {
            if (npcHp <= (npcMaxHp * 0.25f))
            {
                _npcMaster.CallEventNpcLowHealth();
                _criticalHit = true;
            }
            else if (npcHp > (npcMaxHp * 0.25f) && _criticalHit)
            {
                _npcMaster.CallEventNpcHealthRecovered();
                _criticalHit = false;
            }
        }
    }
}
