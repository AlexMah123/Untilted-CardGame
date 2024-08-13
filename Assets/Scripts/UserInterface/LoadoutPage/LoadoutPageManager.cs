using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutPageManager : MonoBehaviour
{
    public static LoadoutPageManager Instance;

    public ActiveLoadoutLayoutManager activeLoadoutLayoutManager;

    [SerializeField] Button previousButton;
    [SerializeField] Button nextButton;

    private int currentPageIndex = 0;

    public event Action<int> OnLoadoutPageUpdatedEvent;

    private void OnEnable()
    {
        activeLoadoutLayoutManager.OnActiveLoadoutUpdatedEvent += UpdateButtonState;
    }

    private void OnDisable()
    {
        activeLoadoutLayoutManager.OnActiveLoadoutUpdatedEvent -= UpdateButtonState;
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
        //null check since this is called multiple times in different timing, could be before things are initialized.
        if (LoadoutLayoutManager.Instance.cachedLoadoutData.totalUpgradesInGame == null) return;

        previousButton.interactable = (currentPageIndex > 0);
        nextButton.interactable = ((currentPageIndex + 1) * LoadoutLayoutManager.Instance.displayAmountPerPage) < LoadoutLayoutManager.Instance.cachedLoadoutData.totalUpgradesInGame.Count;
    }
}
