using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class LoadoutManager : MonoBehaviour, ISavableData
{
    public static LoadoutManager Instance;

    [Header("Total upgrades in game")]
    [SerializeField] List<UpgradeCollectionSO> totalUpgradesInGame = new();

    [Header("Upgrades unlocked by player")]
    [SerializeField] private List<UpgradeDefinitionSO> totalUpgrades = new();
    public List<UpgradeDefinitionSO> cachedUpgradesUnlocked = new();
    public List<UpgradeDefinitionSO> cachedEquippedUpgrades = new();

    //[Header("Player current upgrade")]
    //public Player currentPlayer;

    public event Action<LoadoutData> OnInitializeLoadoutEvent;

    //flag
    private bool isLoadoutSelectedEventBinded = false;

    private void OnEnable()
    {
        if(!isLoadoutSelectedEventBinded)
        {
            BindLoadoutSelectedEvent();
        }
    }

    private void OnDisable()
    {
        if(isLoadoutSelectedEventBinded)
        {
            UnbindLoadoutSelectedEvent();
        }
    }

    private void Awake()
    {
        if(Instance != null && Instance != this)
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

        InitializateData();

        //needs a frame to let singletons exist
        Invoke(nameof(Testing), 0.1f);
    }

    void Testing()
    {
        LoadoutData loadoutData = new LoadoutData(totalUpgrades, cachedUpgradesUnlocked, cachedEquippedUpgrades);
        OnInitializeLoadoutEvent?.Invoke(loadoutData);
    }

    public void HandleSelectedUpgradeToActive(UpgradeDefinitionSO addedUpgradeSO)
    {
        //#TODO: need the cards be added to active
        //#DEBUG
        Debug.Log($"Added {addedUpgradeSO} to player");

        //currentPlayer.ActiveLoadoutComponent.AddToLoadout(addedUpgradeSO);
    }

    #region Savable Interface
    public void LoadData(GameData data)
    {
        //foreach data.upgradeunlocked, add to cachedUpgradesUnlocked
        //foreach data.upgradeunlocked, add to cachedEquippedUpgrades

        Debug.Log($"game data is loaded : {data.number}");

        //Debug.Log($"Loading Data since scene changed {data.number}");
    }

    public void SaveData(ref GameData data)
    {
        //foreach cachedUpgradesUnlocked rewrite data.upgradeunlocked
        //foreach cachedEquippedUpgrades rewrite data.upgradeunlocked

        Debug.Log($"game data is saved : {data.number++}");

        //Debug.Log($"Loading Data since scene changed {data.number}");
    }
    #endregion

    #region Internal Methods
    private void InitializateData()
    {
        totalUpgrades = totalUpgradesInGame.SelectMany(upgradeCollection => upgradeCollection.upgradeList).ToList();
    }
    #endregion

    #region Bind LoadoutSelectedEvent
    private void BindLoadoutSelectedEvent()
    {
        if (LoadoutLayoutManager.Instance != null)
        {
            LoadoutLayoutManager.Instance.OnLoadoutSelectedEvent += HandleSelectedUpgradeToActive;
            isLoadoutSelectedEventBinded = true;
        }
    }

    private void UnbindLoadoutSelectedEvent()
    {
        if (GameManager.Instance != null)
        {
            LoadoutLayoutManager.Instance.OnLoadoutSelectedEvent -= HandleSelectedUpgradeToActive;
            isLoadoutSelectedEventBinded = true;
        }
    }
    #endregion
}
