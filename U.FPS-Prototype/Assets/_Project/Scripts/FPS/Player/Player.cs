/*
 * Player -
 * Created By : Allan Murillo
 * Last Edited : 2/21/2020
 */

using System;
using UnityEngine;

namespace ANM.FPS.Player
{
    public class Player
    {
        private int MaximumHealth { get; }
        public int CurrentHealth { get; private set; }
    
        public event EventHandler<HealedEventArgs> HealedEvent;
        public event EventHandler<HealedEventArgs> DamagedEvent;
    
        public Player(int currentHealth, int maximumHealth = 16)
        {
            if(currentHealth < 0) throw new 
                ArgumentOutOfRangeException(nameof(currentHealth));
            if(currentHealth > maximumHealth) throw new 
                ArgumentOutOfRangeException(nameof(currentHealth));
        
            CurrentHealth = currentHealth;
            MaximumHealth = maximumHealth;
        }

    
        public void Heal(int amount)
        {
            var newHealth = Mathf.Min(CurrentHealth + amount, MaximumHealth);
            HealedEvent?.Invoke(this, new 
                HealedEventArgs(newHealth - CurrentHealth));
            CurrentHealth = newHealth;
        }

        public void Damage(int amount)
        {
            var newHealth = Mathf.Max(CurrentHealth - amount, 0);
            DamagedEvent?.Invoke(this, new 
                HealedEventArgs(CurrentHealth - newHealth));
            CurrentHealth = newHealth;
        }

        public class HealedEventArgs : EventArgs
        {
            public int Amount { get; }
            public HealedEventArgs(int amount)
            {
                Amount = amount;
            }
        }
    }
}
