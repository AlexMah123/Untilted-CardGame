using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public struct LoadoutData
{ 
    public LoadoutData(List<UpgradeDefinitionSO> _totalUpgrades, List<UpgradeDefinitionSO>_totalUnlockedUpgrades)
    {
        totalUpgrades = _totalUpgrades;
        totalUnlockedUpgrades = _totalUnlockedUpgrades;
    }

    public List<UpgradeDefinitionSO> totalUpgrades;
    public List<UpgradeDefinitionSO> totalUnlockedUpgrades;
}


public class LoadoutManager : MonoBehaviour
{
    public static LoadoutManager Instance;

    [Header("Total upgrades in game")]
    [SerializeField] List<UpgradeCollectionSO> totalUpgradesInGame = new();

    [Header("Upgrades unlocked by player")]
    public List<UpgradeDefinitionSO> totalUpgrades = new();
    public List<UpgradeDefinitionSO> totalUnlockedUpgrades = new();

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
        OnInitializeLoadoutEvent?.Invoke(new LoadoutData(totalUpgrades, totalUnlockedUpgrades));
    }

    public void AddSelectedUpgradeToActive()
    {
        //#TODO: need the cards be added to active
        //#DEBUG
        Debug.Log($"Added card to active");
    }

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
            LoadoutLayoutManager.Instance.OnLoadoutSelectedEvent += AddSelectedUpgradeToActive;
            isLoadoutSelectedEventBinded = true;
        }
    }

    private void UnbindLoadoutSelectedEvent()
    {
        if (GameManager.Instance != null)
        {
            LoadoutLayoutManager.Instance.OnLoadoutSelectedEvent -= AddSelectedUpgradeToActive;
            isLoadoutSelectedEventBinded = true;
        }
    }
    #endregion
}
