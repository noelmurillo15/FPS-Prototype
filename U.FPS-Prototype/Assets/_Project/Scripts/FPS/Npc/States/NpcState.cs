/*
 * INpcState -
 * Created by : Allan N. Murillo
 * Last Edited : 3/31/2020
 */

namespace ANM.FPS.Npc.States
{
    public interface INpcState
    {
        void UpdateState();
        void ToPatrolState();
        void ToAlertState();
        void ToChaseState();
        void ToMeleeAttackState();
        void ToRangeAttackState();
    }
}
