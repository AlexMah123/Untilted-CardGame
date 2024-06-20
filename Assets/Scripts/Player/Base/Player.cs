using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(StatComponent), typeof(ChoiceComponent))]
public class Player : MonoBehaviour, IPlayer
{
    public PlayerStatsSO StatsConfig { get => statComponent.stats; }
    public StatComponent StatComponent { get => statComponent; }
    public ChoiceComponent ChoiceComponent { get => choiceComponent; }

    [HideInInspector]
    public GameChoice currentChoice = GameChoice.ROCK;

    //local variables
    private StatComponent statComponent;
    private ChoiceComponent choiceComponent;

    #region Overrides
    protected virtual void Awake()
    {
        statComponent = GetComponent<StatComponent>();
        choiceComponent = GetComponent<ChoiceComponent>();

        //Debug.Log($"{this.GetType().Name} - Health: {statComponent.stats.health}, Damage: {statComponent.stats.damage}, CardSlots: {statComponent.stats.cardSlots}");
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    //overriden by AIPlayer, HumanPlayer is set by turn system manager/GameManager
    public virtual GameChoice GetChoice()
    {
        return currentChoice;
    }
    #endregion
}
