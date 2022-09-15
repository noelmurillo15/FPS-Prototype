/*
 * AoeSpellBehaviour -
 * Created by : Allan N. Murillo
 * Last Edited : 4/7/2020
 */

using UnityEngine;
using ANM.FPS.Npc;
using ANM.FPS.Player;
using ANM.SpellSystem;

namespace RPG.SpellSystem
{
    public class AoeSpellBehaviour : SpellBehaviour
    {
        private void Start()
        {
            caster = GetComponent<PlayerMaster>();
        }

        public override void Activate(GameObject spellParams = null)
        {
            caster.GetComponent<SpellUI>().LoadSpellBehaviour(this, ((AoeSpellConfig) config).GetRadius());
        }

        public static void DealRadialDamage(int baseDmg, Vector3 areaAffected, float radius)
        {
            //  TODO : use non-alloc
            var hits = Physics.SphereCastAll(areaAffected, radius, Vector3.up, radius);

            foreach (var hit in hits)
            {
                var damageable = hit.transform.root.GetComponent<NpcMaster>();

                if (damageable == null) continue;
                Debug.Log("Radial Damage Event has been called : " + baseDmg);
                damageable.CallEventNpcDeductHealth(baseDmg);
            }
        }
    }
}
