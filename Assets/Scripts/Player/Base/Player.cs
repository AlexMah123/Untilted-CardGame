using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(ChoiceComponent), typeof(ActiveLoadoutComponent))]
[RequireComponent(typeof(HealthStatComponent), typeof(DamageStatComponent))]
public class Player : MonoBehaviour, IPlayer
{
    //inherited components from IPlayer
    public ChoiceComponent ChoiceComponent { get => choiceComponent; }
    public ActiveLoadoutComponent ActiveLoadoutComponent { get => activeLoadoutComponent; }
    public HealthStatComponent HealthStatComponent { get => healthStatComponent; }
    public DamageStatComponent DamageStatComponent { get => damageStatComponent; }

    [Header("Player Stats Config")]
    public PlayerStatsSO statsConfig;

    //local variables
    private ChoiceComponent choiceComponent;
    private ActiveLoadoutComponent activeLoadoutComponent;
    private HealthStatComponent healthStatComponent;
    private DamageStatComponent damageStatComponent;


    #region Overrides
    protected virtual void Awake()
    {
        choiceComponent = GetComponent<ChoiceComponent>();
        activeLoadoutComponent = GetComponent<ActiveLoadoutComponent>();
        healthStatComponent = GetComponent<HealthStatComponent>();
        damageStatComponent = GetComponent<DamageStatComponent>();


        //Inject values
        choiceComponent.currentChoice = GameChoice.ROCK;
        activeLoadoutComponent.InitializeComponent(this, statsConfig);
        healthStatComponent.InitializeComponent(statsConfig);
        damageStatComponent.InitializeComponent(statsConfig);
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
        return ChoiceComponent.currentChoice;
    }
    #endregion
}
