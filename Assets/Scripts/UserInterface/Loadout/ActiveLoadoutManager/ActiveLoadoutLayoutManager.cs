using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveLoadoutLayoutManager : MonoBehaviour
{
    public GameObject activeLoadoutParent;
    public LoadoutLayoutManager loadoutLayoutManager;

    public event Action OnActiveLoadoutUpdatedEvent;

    public event Action<LoadoutCardGOInfo> OnActiveLoadoutRemovedEvent;

    private void OnEnable()
    {
        LoadoutCardUI[] loadoutCards = activeLoadoutParent.GetComponentsInChildren<LoadoutCardUI>(includeInactive: true);

        foreach (var loadoutCard in loadoutCards)
        {
            loadoutCard.OnCardRemovedEvent += HandleCardRemoved;
        }
    }

    private void OnDisable()
    {
        if(activeLoadoutParent != null)
        {
            LoadoutCardUI[] loadoutCards = activeLoadoutParent.GetComponentsInChildren<LoadoutCardUI>(includeInactive: true);

            foreach (var loadoutCard in loadoutCards)
            {
                if(loadoutCard != null)
                {
                    loadoutCard.OnCardRemovedEvent -= HandleCardRemoved;
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
        OnActiveLoadoutRemovedEvent?.Invoke(loadoutCardInfo);
        OnActiveLoadoutUpdatedEvent?.Invoke();
    }

    #region Internal methods
    private void InitializeActiveLoadout()
    {
        var activeLoadoutGOList = GetAllActiveLoadoutGO();

        foreach (var loadoutGO in activeLoadoutGOList)
        {
            loadoutGO.SetActive(false);
        }

        OnActiveLoadoutUpdatedEvent?.Invoke();
    }

    private GameObject GetFirstInactiveLoadout()
    {
        var activeLoadoutGOList = GetAllActiveLoadoutGO();

        foreach(var loadoutGO in activeLoadoutGOList)
        {
            if(!loadoutGO.activeSelf)
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
