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

    private int currentPageIndex = 0;

    public event Action<int> OnLoadoutPageUpdatedEvent;

    private void OnEnable()
    {
        ActiveLoadoutLayoutManager.OnActiveLoadoutUpdatedEvent += UpdateButtonState;
    }

    private void OnDisable()
    {
        ActiveLoadoutLayoutManager.OnActiveLoadoutUpdatedEvent -= UpdateButtonState;
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
        Invoke(nameof(UpdateButtonState), 0.1f);
    }

    public void PreviousPage()
    {
        currentPageIndex--;
        UpdateButtonState();

        //primarily binded to LoadoutLayoutManager
        OnLoadoutPageUpdatedEvent?.Invoke(currentPageIndex);
    }

    public void NextPage()
    {
        currentPageIndex++;
        UpdateButtonState();

        //primarily binded to LoadoutLayoutManager
        OnLoadoutPageUpdatedEvent?.Invoke(currentPageIndex);
    }

    public void UpdateButtonState()
    {
        previousButton.interactable = (currentPageIndex == 0) ? false : true;
        nextButton.interactable = (currentPageIndex + 1) * LoadoutLayoutManager.Instance.displayAmountPerPage < LoadoutLayoutManager.Instance.loadoutData.totalUpgrades.Count;
    }
}
