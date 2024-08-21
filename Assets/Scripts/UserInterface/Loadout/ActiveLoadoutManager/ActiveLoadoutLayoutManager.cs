using System;
using System.Collections.Generic;
using UnityEngine;

public class ActiveLoadoutLayoutManager : MonoBehaviour
{
    public GameObject activeLoadoutParent;
    public LoadoutLayoutManager loadoutLayoutManager;

    public event Action OnLoadoutUpdated;

    public event Action<LoadoutCardGOInfo> OnLoadoutRemoved;

    private void OnEnable()
    {
        LoadoutCardUI[] loadoutCards = activeLoadoutParent.GetComponentsInChildren<LoadoutCardUI>(includeInactive: true);

        foreach (LoadoutCardUI loadoutCard in loadoutCards)
        {
            loadoutCard.OnCardRemoved += HandleCardRemoved;
        }
    }

    private void OnDisable()
    {
        if (activeLoadoutParent != null)
        {
            LoadoutCardUI[] loadoutCards = activeLoadoutParent.GetComponentsInChildren<LoadoutCardUI>(includeInactive: true);

            foreach (LoadoutCardUI loadoutCard in loadoutCards)
            {
                if (loadoutCard != null)
                {
                    loadoutCard.OnCardRemoved -= HandleCardRemoved;
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

        foreach (Transform child in activeLoadoutParent.transform)
        {
            loadoutGO.Add(child.gameObject);
        }

        return loadoutGO;
    }


    #endregion
}
