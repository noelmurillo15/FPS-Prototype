/*
 * PlayerHealth -
 * Created By : Allan Murillo
 * Last Edited : 2/21/2020
 */

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ANM.FPS.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private List<Image> images;
        [SerializeField] private int amount;
        
        private HeartContainer _heartContainer;
        private PlayerMaster _playerMaster;
        private Player _player;
        

        private void Initialize()
        {
            _playerMaster = GetComponent<PlayerMaster>();
            
            _player = new Player(9);
            _player.HealedEvent += (sender, args) => _heartContainer.Replenish(args.Amount);
            _player.DamagedEvent += (sender, args) => _heartContainer.Deplete(args.Amount);
            
            _heartContainer = new HeartContainer(images.Select(image => 
                new Heart(image)).ToList());
            
            _heartContainer.SetHearts(_player.CurrentHealth);
        }

        private void OnEnable()
        {
            Initialize();
            _playerMaster.PlayerHpDeductionEvent += DeductHealth;
            _playerMaster.PlayerHpIncreaseEvent += IncreaseHealth;
        }

        private void OnDisable()
        {
            _playerMaster.PlayerHpDeductionEvent -= DeductHealth;
            _playerMaster.PlayerHpIncreaseEvent -= IncreaseHealth;
        }

        public void DeductHealth(float hp)
        {
            amount = (int)hp;
            _player.Damage(amount);
            if (_player.CurrentHealth <= 0)
            {
                _playerMaster.EventCallPlayerDie();
            }
        }

        private void IncreaseHealth(float hp)
        {
            amount = (int)hp;
            _player.Heal(amount);
        }
    }
}