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
    public List<UpgradeDefinitionSO> totalUpgrades = new();
    public List<UpgradeDefinitionSO> totalUnlockedUpgrades = new();

    [Header("Player current upgrade")]
    public Player currentPlayer;

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
        if(!isLoadoutSelectedEventBinded)
        {
            BindLoadoutSelectedEvent();
        }

        InitializateData();

        //#TODO: Add Players active upgrades in, 3rd param
        LoadoutData loadoutData = new LoadoutData(totalUpgrades, totalUnlockedUpgrades, new());
        OnInitializeLoadoutEvent?.Invoke(loadoutData);
    }

    public void HandleSelectedUpgradeToActive(UpgradeDefinitionSO addedUpgradeSO)
    {
        //#TODO: need the cards be added to active
        //#DEBUG
        Debug.Log($"Added upgrade to player");

        //currentPlayer.ActiveLoadoutComponent.AddToLoadout(addedUpgradeSO);
    }

    #region Savable Interface
    public void LoadData(GameData data)
    {
        Debug.Log($"Loading Data since scene changed {data.number}");
    }

    public void SaveData(ref GameData data)
    {
        data.number++;
        Debug.Log($"Loading Data since scene changed {data.number}");
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
