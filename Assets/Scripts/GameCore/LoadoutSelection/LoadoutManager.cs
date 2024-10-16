using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.SaveSystem;
using GameCore.SaveSystem.Data;
using PlayerCore;
using PlayerCore.Upgrades.Base;
using PlayerCore.Upgrades.UpgradeCollection;
using PlayerCore.Upgrades.UpgradeFactory;
using UnityEngine;
using UserInterface.LoadoutSelection;

namespace GameCore.LoadoutSelection
{
    public class LoadoutManager : MonoBehaviour, ISavableData
    {
        public static LoadoutManager Instance;

        [Header("Loadout Configs")] 
        [SerializeField] private PlayerStatsSO playerStats;
        
        [Header("Total upgrades in game")]
        [SerializeField] private List<UpgradeCollectionSO> totalUpgradesInGame = new();

        //from runtime, loaded from totalUpgradesInGame
        private List<UpgradeDefinitionSO> totalUpgrades = new();

        //from savedata
        public HashSet<UpgradeDefinitionSO> cachedUpgradesUnlocked = new();
        public HashSet<UpgradeDefinitionSO> cachedEquippedUpgrades = new();
        public int cachedMaxSlots;
        
        public event Action<FLoadoutData> OnInitializeLoadout;

        //Interface
        public event Action OnSaveDataLoaded;

        //flag
        private bool isLoadoutSelectedEventBinded = false;
        private bool isEquippedLoadoutRemovedEvent = false;

        private void OnEnable()
        {
            if (!isLoadoutSelectedEventBinded)
            {
                BindLoadoutSelectedEvent();
            }

            if (!isEquippedLoadoutRemovedEvent)
            {
                BindEquippedLoadoutRemovedEvent();
            }
        }

        private void OnDisable()
        {
            if (isLoadoutSelectedEventBinded)
            {
                UnbindLoadoutSelectedEvent();
            }

            if (isEquippedLoadoutRemovedEvent)
            {
                UnbindEquippedLoadoutRemovedEvent();
            }
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            //to avoid racing condition
            if (!isLoadoutSelectedEventBinded)
            {
                BindLoadoutSelectedEvent();
            }

            if (!isEquippedLoadoutRemovedEvent)
            {
                BindEquippedLoadoutRemovedEvent();
            }

            InitializeUpgradeData();

            //needs a frame to let singletons exist
            Invoke(nameof(InitializeLoadoutData), 0.1f);
        }

        void InitializeLoadoutData()
        {
            FLoadoutData fLoadoutData = new FLoadoutData(totalUpgrades, cachedUpgradesUnlocked, cachedEquippedUpgrades);

            OnInitializeLoadout?.Invoke(fLoadoutData);
        }

        public void HandleSelectedUpgradeEquipped(UpgradeDefinitionSO addedUpgradeSO)
        {
            //remove from the total list available
            totalUpgrades.Remove(addedUpgradeSO);

            //add to the equipped upgrades
            cachedEquippedUpgrades.Add(addedUpgradeSO);

            //#DEBUG
            //Debug.Log($"Added {addedUpgradeSO.upgradeName} to player");
        }

        public void HandleEquippedUpgradeRemoved(UpgradeDefinitionSO addedUpgradeSO)
        {
            //Add to the total list available
            totalUpgrades.Add(addedUpgradeSO);

            //remove from equipped upgrades
            cachedEquippedUpgrades.Remove(addedUpgradeSO);

            //#DEBUG
            //Debug.Log($"Removed {addedUpgradeSO.upgradeName} from player");
        }

        #region Savable Interface
        public void LoadData(GameData data)
        {
            InitializeLoadoutData(ref data);

            //#DEBUG
            //Debug.Log($"game data is loaded");

            OnSaveDataLoaded?.Invoke();
        }

        public void SaveData(ref GameData data)
        {
            SavePlayerUpgradeData(ref data);

            //#DEBUG
            //Debug.Log($"game data is saved");
        }

        #endregion

        #region Internal Methods
        private void SavePlayerUpgradeData(ref GameData data)
        {
            data.playerUnlockedUpgrades.Clear();
            foreach (UpgradeDefinitionSO upgradeSO in cachedUpgradesUnlocked)
            {
                data.playerUnlockedUpgrades.Add(upgradeSO.upgradeType);
            }

            data.playerEquippedUpgrades.Clear();
            foreach (UpgradeDefinitionSO upgradeSO in cachedEquippedUpgrades)
            {
                data.playerEquippedUpgrades.Add(upgradeSO.upgradeType);
            }
        }
        
        private void InitializeLoadoutData(ref GameData data)
        {
            cachedMaxSlots = data.upgradedPlayerStats.cardSlots + playerStats.cardSlots;
            
            //clear when loading to overwrite
            cachedUpgradesUnlocked.Clear();
            foreach (UpgradeType upgradeUnlocked in data.playerUnlockedUpgrades)
            {
                cachedUpgradesUnlocked.Add(UpgradeSOFactory.CreateUpgradeDefinitionSO(upgradeUnlocked));
            }

            cachedEquippedUpgrades.Clear();
            foreach (UpgradeType upgradesEquipped in data.playerEquippedUpgrades)
            {
                cachedEquippedUpgrades.Add(UpgradeSOFactory.CreateUpgradeDefinitionSO(upgradesEquipped));
            }
        }
        
        private void InitializeUpgradeData()
        {
            totalUpgrades = totalUpgradesInGame.SelectMany(upgradeCollection => upgradeCollection.upgradeList).ToList();
        }
        #endregion

        #region Bind LoadoutSelectedEvent
        private void BindLoadoutSelectedEvent()
        {
            if (LoadoutSelectionManager.Instance != null)
            {
                LoadoutSelectionManager.Instance.OnLoadoutSelected += HandleSelectedUpgradeEquipped;
                isLoadoutSelectedEventBinded = true;
            }
        }

        private void UnbindLoadoutSelectedEvent()
        {
            if (LoadoutSelectionManager.Instance != null)
            {
                LoadoutSelectionManager.Instance.OnLoadoutSelected -= HandleSelectedUpgradeEquipped;
                isLoadoutSelectedEventBinded = false;
            }
        }
        #endregion
        
        #region Bind EquippedLoadoutRemovedEvent
        private void BindEquippedLoadoutRemovedEvent()
        {
            if (LoadoutSelectionManager.Instance != null)
            {
                LoadoutSelectionManager.Instance.OnEquippedLoadoutRemoved += HandleEquippedUpgradeRemoved;
                isEquippedLoadoutRemovedEvent = true;
            }
        }

        private void UnbindEquippedLoadoutRemovedEvent()
        {
            if (LoadoutSelectionManager.Instance != null)
            {
                LoadoutSelectionManager.Instance.OnEquippedLoadoutRemoved -= HandleEquippedUpgradeRemoved;
                isEquippedLoadoutRemovedEvent = true;
            }
        }
        #endregion
    }
}
