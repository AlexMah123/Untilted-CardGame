using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutPageManager : MonoBehaviour
{
    public static LoadoutPageManager Instance;

    [SerializeField] Button previousButton;
    [SerializeField] Button nextButton;

    public int currentPageIndex = 1;

    public event Action<int> OnLoadoutPageUpdatedEvent;

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
        UpdateButtonState();
    }

    public void PreviousPage()
    {
        currentPageIndex--;
        UpdateButtonState();

        //call update or something
        OnLoadoutPageUpdatedEvent?.Invoke(currentPageIndex);
    }

    public void NextPage()
    {
        currentPageIndex++;
        UpdateButtonState();

        //call update or something
        OnLoadoutPageUpdatedEvent?.Invoke(currentPageIndex);
    }

    private void UpdateButtonState()
    {
        previousButton.interactable = (currentPageIndex == 0) ? false : true;
        nextButton.interactable = (currentPageIndex + 1) * LoadoutLayoutManager.Instance.displayAmountPerPage < LoadoutLayoutManager.Instance.loadoutData.totalUpgrades.Count;
    }
}
