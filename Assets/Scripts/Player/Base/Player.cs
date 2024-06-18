using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(StatComponent))]
public class Player : MonoBehaviour, IPlayer
{

    public PlayerStatsSO StatsConfig { get => statComponent.stats; }
    public StatComponent StatComponent { get => statComponent; }


    private StatComponent statComponent;

    #region Overrides
    protected virtual void Awake()
    {
        statComponent = GetComponent<StatComponent>();

        //Debug.Log($"{this.GetType().Name} - Health: {statComponent.stats.health}, Damage: {statComponent.stats.damage}, CardSlots: {statComponent.stats.cardSlots}");
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    public virtual void GetChoice()
    {
        
    }
    #endregion
}

public enum GameChoice
{
    NONE,
    ROCK,
    PAPER,
    SCISSOR
}
