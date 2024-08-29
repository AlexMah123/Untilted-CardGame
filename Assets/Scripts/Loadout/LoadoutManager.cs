using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class LoadoutManager : MonoBehaviour, ISavableData
{
    public static LoadoutManager Instance;

    [Header("Total upgrades in game")]
    [SerializeField] List<UpgradeCollectionSO> totalUpgradesInGame = new();

    [Header("Upgrades unlocked by player")]
    [SerializeField] private List<UpgradeDefinitionSO> totalUpgrades = new();
    public HashSet<UpgradeDefinitionSO> cachedUpgradesUnlocked = new();
    public HashSet<UpgradeDefinitionSO> cachedEquippedUpgrades = new();

    public event Action<LoadoutData> OnInitializeLoadout;

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

        InitializateUpgradeData();

        //needs a frame to let singletons exist
        Invoke(nameof(InitializeLoadoutData), 0.1f);
    }

    void InitializeLoadoutData()
    {
        LoadoutData loadoutData = new LoadoutData(totalUpgrades, cachedUpgradesUnlocked, cachedEquippedUpgrades);

        OnInitializeLoadout?.Invoke(loadoutData);
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

        //#DEBUG
        //Debug.Log($"game data is loaded");

        OnSaveDataLoaded?.Invoke();
    }

    public void SaveData(ref GameData data)
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

        //#DEBUG
        //Debug.Log($"game data is saved");
    }
    #endregion

    #region Internal Methods
    private void InitializateUpgradeData()
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
