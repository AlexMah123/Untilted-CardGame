using System;
using UnityEngine;

namespace PlayerCore
{
    [CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "Player/PlayerStatsSO")]
    public class PlayerStatsSO : ScriptableObject
    {
        [Header("Stats Config")] 
        public int maxHealth;
        public int attack;
        public int damageTaken;
        public int cardSlots;
        public int energy;

        public PlayerStatsSO(int _maxHealth, int _attack, int _damageTaken, int _cardSlots, int _energy)
        {
            maxHealth = _maxHealth;
            attack = _attack;
            damageTaken = _damageTaken;
            cardSlots = _cardSlots;
            energy = _energy;
        }

        public PlayerStatsSO()
        {
            maxHealth = 0;
            attack = 0;
            damageTaken = 0;
            cardSlots = 0;
            energy = 0;
        }
    }

    [Serializable]
    public class PlayerStats
    {
        [Header("Stats Config")] 
        public int maxHealth;
        public int attack;
        public int damageTaken;
        public int cardSlots;
        public int energy;

        public PlayerStats(int _maxHealth, int _attack, int _damageTaken, int _cardSlots, int _energy)
        {
            maxHealth = _maxHealth;
            attack = _attack;
            damageTaken = _damageTaken;
            cardSlots = _cardSlots;
            energy = _energy;
        }

        public PlayerStats()
        {
            maxHealth = 0;
            attack = 0;
            damageTaken = 0;
            cardSlots = 0;
            energy = 0;
        }
    }
}