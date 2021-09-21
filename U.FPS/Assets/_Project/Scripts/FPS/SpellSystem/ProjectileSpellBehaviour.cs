/*
 * ProjectileSpellBehaviour -
 * Created by : Allan N. Murillo
 * Last Edited : 4/7/2020
 */

using UnityEngine;
using ANM.FPS.Player;

namespace RPG.SpellSystem
{
    public class ProjectileSpellBehaviour : SpellBehaviour
    {
        private void Start()
        {
            caster = GetComponent<PlayerMaster>();
        }

        public override void Activate(GameObject spellParams = null)
        {
            FireProjectile(spellParams);
        }

        private void FireProjectile(GameObject spellParams)
        {
            PlayParticleEffect();
        }
    }
}
