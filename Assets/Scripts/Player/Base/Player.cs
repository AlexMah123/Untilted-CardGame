using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(ChoiceComponent), typeof(ActiveLoadoutComponent))]
[RequireComponent(typeof(HealthComponent), typeof(DamageComponent), typeof(EnergyComponent))]
public class Player : MonoBehaviour, IPlayer
{
    //inherited components from IPlayer
    public ChoiceComponent ChoiceComponent { get => choiceComponent; }
    public ActiveLoadoutComponent ActiveLoadoutComponent { get => activeLoadoutComponent; }
    public HealthComponent HealthComponent { get => healthComponent; }
    public DamageComponent DamageComponent { get => damageComponent; }
    public EnergyComponent EnergyComponent { get => energyComponent; }

    [Header("Player Stats Config")]
    public PlayerStatsSO statsConfig;

    //local variables
    private ChoiceComponent choiceComponent;
    private ActiveLoadoutComponent activeLoadoutComponent;
    private HealthComponent healthComponent;
    private DamageComponent damageComponent;
    private EnergyComponent energyComponent;


    #region Overrides
    protected virtual void Awake()
    {
        choiceComponent = GetComponent<ChoiceComponent>();
        activeLoadoutComponent = GetComponent<ActiveLoadoutComponent>();
        healthComponent = GetComponent<HealthComponent>();
        damageComponent = GetComponent<DamageComponent>();
        energyComponent = GetComponent<EnergyComponent>();

        LoadComponents();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    //override if neccessary
    public virtual void LoadComponents()
    {
        //Inject values
        choiceComponent.currentChoice = GameChoice.Rock;
        activeLoadoutComponent.InitializeComponent(this, statsConfig);
        healthComponent.InitializeComponent(statsConfig);
        damageComponent.InitializeComponent(statsConfig);
        energyComponent.InitializeComponent(statsConfig);
    }

    //overriden by AIPlayer, HumanPlayer is set by turn system manager/GameManager
    public virtual GameChoice GetChoice()
    {
        return ChoiceComponent.currentChoice;
    }
    #endregion
}
