using System;
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

        public PlayerStats upgradeStats = new();
        public PlayerStats currentStats = new();

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

        protected virtual void Update()
        {

        }

        //override if necessary
        public virtual void LoadComponents()
        {
            UpdateCurrentStats();
        
            //Inject values
            choiceComponent.currentChoice = GameChoice.Rock;
            activeLoadoutComponent.InitializeComponent(this, currentStats);
            healthComponent.InitializeComponent(currentStats);
            damageComponent.InitializeComponent(currentStats);
            energyComponent.InitializeComponent(currentStats);
        }

        //overriden by AIPlayer, HumanPlayer is set by turn system manager/GameManager
        public virtual GameChoice GetChoice()
        {
            return ChoiceComponent.currentChoice;
        }

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
        #endregion

        //override to have their own implementation of loadData
        protected virtual void LoadPlayerData(GameData data)
        {
            //implement in child class
        }

        public void UpdateCurrentStats()
        {
            currentStats.health = baseStatsConfig.health + upgradeStats.health;
            currentStats.damage = baseStatsConfig.damage + upgradeStats.damage;
            currentStats.cardSlots = baseStatsConfig.cardSlots + upgradeStats.cardSlots;
            currentStats.energy = baseStatsConfig.energy + upgradeStats.energy;
        }
    }
}
