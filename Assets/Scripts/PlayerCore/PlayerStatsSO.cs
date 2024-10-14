using System;
using UnityEngine;

namespace PlayerCore
{
    [CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "Player/PlayerStatsSO")]
    public class PlayerStatsSO : ScriptableObject
    {
        [Header("Stats Config")]
        public int health;
        public int damage;
        public int cardSlots;
        public int energy;

        public PlayerStatsSO(int _health, int _damage, int _cardSlots, int _energy)
        {
            health = _health;
            damage = _damage;
            cardSlots = _cardSlots;
            energy = _energy;
        }

        public PlayerStatsSO()
        {
            health = 0;
            damage = 0;
            cardSlots = 0;
            energy = 0;
        }
    }

    [Serializable]
    public class PlayerStats
    {
        [Header("Stats Config")]
        public int health;
        public int damage;
        public int cardSlots;
        public int energy;

        public PlayerStats()
        {
            health = 0;
            damage = 0;
            cardSlots = 0;
            energy = 0;
        }
    
        public PlayerStats(int _health, int _damage, int _cardSlots, int _energy)
        {
            health = _health;
            damage = _damage;
            cardSlots = _cardSlots;
            energy = _energy;
        }
    }
}