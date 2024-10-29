using System;
using GameCore;
using GameCore.SaveSystem;
using GameCore.SaveSystem.Data;
using LevelCore.LevelManager;
using PlayerCore.PlayerComponents;
using UnityEngine;

namespace PlayerCore
{
    [RequireComponent(typeof(ChoiceComponent), typeof(ActiveLoadoutComponent))]
    [RequireComponent(typeof(HealthComponent), typeof(DamageComponent), typeof(EnergyComponent))]
    public class Player : MonoBehaviour, IPlayer, ISavableData
    {
        //inherited components from IPlayer
        public ChoiceComponent ChoiceComponent => choiceComponent;
        public ActiveLoadoutComponent ActiveLoadoutComponent => activeLoadoutComponent;
        public HealthComponent HealthComponent => healthComponent;
        public DamageComponent DamageComponent => damageComponent;
        public EnergyComponent EnergyComponent => energyComponent;

        [Header("Player Stats Config")] 
        public PlayerStatsSO baseStatsConfig;
        public PlayerStats progressionStats = new();
        public PlayerStats cardStats = new();
        public PlayerStats finalStats = new();

        //local variables
        private ChoiceComponent choiceComponent;
        private ActiveLoadoutComponent activeLoadoutComponent;
        private HealthComponent healthComponent;
        private DamageComponent damageComponent;
        private EnergyComponent energyComponent;

        //Interface
        public event Action OnSaveDataLoaded;

        #region Overrides
        protected virtual void Awake()
        {
            choiceComponent = GetComponent<ChoiceComponent>();
            activeLoadoutComponent = GetComponent<ActiveLoadoutComponent>();
            healthComponent = GetComponent<HealthComponent>();
            damageComponent = GetComponent<DamageComponent>();
            energyComponent = GetComponent<EnergyComponent>();
        }

        protected virtual void Start()
        {
            LoadComponents();
        }

        //override if necessary
        protected virtual void LoadComponents()
        {
            InitializeStartingStats();

            //Inject values
            choiceComponent.currentChoice = GameChoice.Rock;
            healthComponent.InitializeComponent(finalStats);
            damageComponent.InitializeComponent(finalStats);
            energyComponent.InitializeComponent(finalStats);

            //active component is overriden to specify
        }

        //overriden by AIPlayer, HumanPlayer is set by turn system manager/GameManager
        public virtual GameChoice GetChoice()
        {
            return ChoiceComponent.currentChoice;
        }
        #endregion

        #region SaveSystem
        public virtual void LoadData(GameData data)
        {
            if (LevelDataManager.Instance.currentSelectedLevelSO == null)
            {
                throw new NullReferenceException("LevelDataManager does not have a levelSO selected");
            }

            LoadPlayerData(data);

            OnSaveDataLoaded?.Invoke();
        }

        public virtual void SaveData(ref GameData data)
        {
            
        }

        //implement in child class to have their own implementation of loadData
        protected virtual void LoadPlayerData(GameData data)
        {
            
        }
        #endregion
        
        private void InitializeStartingStats()
        {
            cardStats = ActiveLoadoutComponent.ApplyStatUpgrade(cardStats);

            finalStats.maxHealth = baseStatsConfig.maxHealth + progressionStats.maxHealth + cardStats.maxHealth;
            finalStats.attack = baseStatsConfig.attack + progressionStats.attack + cardStats.attack;
            finalStats.damageTaken = baseStatsConfig.damageTaken + progressionStats.damageTaken + cardStats.damageTaken;
            finalStats.cardSlots = baseStatsConfig.cardSlots + progressionStats.cardSlots + cardStats.cardSlots;
            finalStats.energy = baseStatsConfig.energy + progressionStats.energy + cardStats.energy;
        }

        public void UpdateCurrentStats()
        {
            cardStats = ActiveLoadoutComponent.ApplyStatUpgrade(cardStats);

            finalStats.maxHealth = baseStatsConfig.maxHealth + progressionStats.maxHealth + cardStats.maxHealth;
            finalStats.attack = baseStatsConfig.attack + progressionStats.attack + cardStats.attack;
            finalStats.damageTaken = baseStatsConfig.damageTaken + progressionStats.damageTaken + cardStats.damageTaken;
            finalStats.cardSlots = baseStatsConfig.cardSlots + progressionStats.cardSlots + cardStats.cardSlots;
            finalStats.energy = baseStatsConfig.energy + progressionStats.energy + cardStats.energy;

            //update the components
            healthComponent.UpdateComponent(finalStats);
            damageComponent.InitializeComponent(finalStats);
            energyComponent.InitializeComponent(finalStats);
        }
    }
}