using System;
using System.Collections.Generic;
using UnityEngine;

public class EquippedLoadoutSelectionManager : MonoBehaviour
{
    public GameObject equippedLoadoutParent;
    public LoadoutSelectionManager loadoutLayoutManager;

    public event Action OnLoadoutUpdated;

    public event Action<LoadoutCardGOInfo> OnLoadoutRemoved;

    //#TODO: Add limit/checking of equippable amount for loadout

    private void OnEnable()
    {
        LoadoutCardUI[] loadoutCards = equippedLoadoutParent.GetComponentsInChildren<LoadoutCardUI>(includeInactive: true);

        foreach (LoadoutCardUI loadoutCard in loadoutCards)
        {
            loadoutCard.OnCardClicked += HandleCardRemoved;
        }
    }

    private void OnDisable()
    {
        if (equippedLoadoutParent != null)
        {
            LoadoutCardUI[] loadoutCards = equippedLoadoutParent.GetComponentsInChildren<LoadoutCardUI>(includeInactive: true);

            foreach (LoadoutCardUI loadoutCard in loadoutCards)
            {
                if (loadoutCard != null)
                {
                    loadoutCard.OnCardClicked -= HandleCardRemoved;
                }
            }
        }
    }

    private void Start()
    {
        InitializeActiveLoadout();
    }

    public bool AddUpgrade(UpgradeDefinitionSO selectedUpgrade)
    {
        GameObject loadoutCardGO = GetFirstInactiveLoadout();

        if (loadoutCardGO)
        {
            LoadoutCardUI loadoutCardUI = loadoutCardGO.GetComponent<LoadoutCardUI>();

            if (loadoutCardUI)
            {
                loadoutCardUI.InitializeCard(new LoadoutCardGOInfo(selectedUpgrade));
                loadoutCardGO.SetActive(true);

                return true;
            }
        }

        //fallback defaults to false.
        return false;
    }

    public void HandleCardRemoved(LoadoutCardGOInfo loadoutCardInfo)
    {
        OnLoadoutRemoved?.Invoke(loadoutCardInfo);
        OnLoadoutUpdated?.Invoke();
    }

    #region Internal methods
    private void InitializeActiveLoadout()
    {
        var activeLoadoutGOList = GetAllActiveLoadoutGO();

        foreach (GameObject loadoutGO in activeLoadoutGOList)
        {
            loadoutGO.SetActive(false);
        }

        OnLoadoutUpdated?.Invoke();
    }

    private GameObject GetFirstInactiveLoadout()
    {
        var activeLoadoutGOList = GetAllActiveLoadoutGO();

        foreach (GameObject loadoutGO in activeLoadoutGOList)
        {
            if (!loadoutGO.activeSelf)
            {
                return loadoutGO;
            }
        }

        return null;
    }

    private List<GameObject> GetAllActiveLoadoutGO()
    {
        List<GameObject> loadoutGO = new();

        foreach (Transform child in equippedLoadoutParent.transform)
        {
            loadoutGO.Add(child.gameObject);
        }

        return loadoutGO;
    }


    #endregion
}
