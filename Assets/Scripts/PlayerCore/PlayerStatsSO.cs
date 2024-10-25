﻿using System;
using UnityEngine;

namespace PlayerCore
{
    [CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "Player/PlayerStatsSO")]
    public class PlayerStatsSO : ScriptableObject
    {
        [Header("Stats Config")] 
        public int maxHealth;
        public int damage;
        public int cardSlots;
        public int energy;

        public PlayerStatsSO(int _maxHealth, int _damage, int _cardSlots, int _energy)
        {
            maxHealth = _maxHealth;
            damage = _damage;
            cardSlots = _cardSlots;
            energy = _energy;
        }

        public PlayerStatsSO()
        {
            maxHealth = 0;
            damage = 0;
            cardSlots = 0;
            energy = 0;
        }
    }

    [Serializable]
    public class PlayerStats
    {
        [Header("Stats Config")]
        public int maxHealth;
        public int damage;
        public int cardSlots;
        public int energy;
        
        public PlayerStats(int _maxHealth, int _damage, int _cardSlots, int _energy)
        {
            maxHealth = _maxHealth;
            damage = _damage;
            cardSlots = _cardSlots;
            energy = _energy;
        }
        
        public PlayerStats()
        {
            maxHealth = 0;
            damage = 0;
            cardSlots = 0;
            energy = 0;
        }
    

    }
}