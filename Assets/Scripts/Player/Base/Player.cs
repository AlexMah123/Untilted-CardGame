using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(StatComponent), typeof(ChoiceComponent), typeof(ActiveLoadoutComponent))]
public class Player : MonoBehaviour, IPlayer
{
    //components
    public StatComponent StatComponent { get => statComponent; }
    public ChoiceComponent ChoiceComponent { get => choiceComponent; }
    public ActiveLoadoutComponent ActiveLoadoutComponent { get => activeLoadoutComponent; }

    public PlayerStatsSO StatsConfig { get => statComponent.stats; }

    [HideInInspector]
    public GameChoice currentChoice = GameChoice.ROCK;

    //local variables
    private StatComponent statComponent;
    private ChoiceComponent choiceComponent;
    private ActiveLoadoutComponent activeLoadoutComponent;

    #region Overrides
    protected virtual void Awake()
    {
        statComponent = GetComponent<StatComponent>();
        choiceComponent = GetComponent<ChoiceComponent>();
        activeLoadoutComponent = GetComponent<ActiveLoadoutComponent>();
        activeLoadoutComponent.attachedPlayer = this;


        currentChoice = GameChoice.ROCK;
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
