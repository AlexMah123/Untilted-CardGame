using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}