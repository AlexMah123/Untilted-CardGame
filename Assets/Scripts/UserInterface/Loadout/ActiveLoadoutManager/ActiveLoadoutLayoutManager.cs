using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveLoadoutLayoutManager : MonoBehaviour
{
    public GameObject activeLoadoutParent;

    public static event Action OnActiveLoadoutUpdatedEvent;

    private void Start()
    {
        InitializeActiveLoadout();

    }

    public bool UpdateLayout(UpgradeDefinitionSO selectedUpgrade)
    {
        GameObject loadoutCardGO = GetFirstInactiveLoadout();

        if (loadoutCardGO)
        {
            LoadoutCardUI loadoutCardUI = loadoutCardGO.GetComponent<LoadoutCardUI>();

            if (loadoutCardUI)
            {
                loadoutCardUI.InitializeCard(selectedUpgrade);
                loadoutCardGO.SetActive(true);

                OnActiveLoadoutUpdatedEvent?.Invoke();
                return true;
            }
        }

        //fallback defaults to false.
        return false;
    }

    #region Internal methods
    private void InitializeActiveLoadout()
    {
        var activeLoadoutGOList = GetAllActiveLoadoutGO();

        foreach (var loadoutGO in activeLoadoutGOList)
        {
            loadoutGO.SetActive(false);
        }
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
