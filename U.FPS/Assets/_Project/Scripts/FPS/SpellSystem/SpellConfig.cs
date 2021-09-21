/*
 * SpellConfig -
 * Created by : Allan N. Murillo
 * Last Edited : 4/7/2020
 */

using UnityEngine;

namespace RPG.SpellSystem
{
    public enum BuffType
    {
        HP,
        MANA,
    }

    public abstract class SpellConfig : ScriptableObject
    {
        #region Variables

        [Header("Spell General Settings")] 
        [SerializeField] private float manaCost = 10f;
        [SerializeField] private AudioClip audio = null;
        [SerializeField] private AnimationClip animation = null;
        [SerializeField] private GameObject particlePrefab = null;
        [SerializeField] private Sprite skillIcon = null;
        
        private SpellBehaviour behaviour;

        #endregion


        protected abstract SpellBehaviour GetUniqueBehaviour(GameObject objAttached);

        public void Activate()
        {
            behaviour.Activate();
        }

        public void AttachSpell(GameObject objAttached)
        {
            SpellBehaviour behaviourComponent = GetUniqueBehaviour(objAttached);
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }

        #region Accessors

        public float GetManaCost() => manaCost;
        public AudioClip GetAudio() => audio;
        public AnimationClip GetAnimation() => animation;
        public GameObject GetParticles() => particlePrefab;
        public Sprite GetSpellIcon() => skillIcon;

        #endregion
    }
}
