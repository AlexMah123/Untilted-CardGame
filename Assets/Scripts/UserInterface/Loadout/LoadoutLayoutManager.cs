using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(LoadoutCardFactory))]
public class LoadoutLayoutManager : MonoBehaviour
{
    public static LoadoutLayoutManager Instance;

    [Header("ActiveLoadoutLayoutManager")]
    public ActiveLoadoutLayoutManager ActiveLoadoutLayoutManager;

    [Header("Layout Config")]
    public Transform spawnContainer;
    public float radiusFromCenter;
    public float angleBetweenCards;
    [Range(1, 9)] public int displayAmountPerPage = 9;

    [Header("Layout Offset Config")]
    [Tooltip("This value is relative to World Position")] public Vector3 offsetPosition;

    public LoadoutData cachedLoadoutData;

    //private
    private List<LoadoutCardGO> cardSlots = new();
    private int loadoutPageIndex;

    //factory
    private LoadoutCardFactory upgradeCardFactory;

    //event
    public event Action<UpgradeDefinitionSO> OnLoadoutSelectedEvent;

    //flag
    private bool isInitializeLoadoutEventBinded = false;
    private bool isLoadoutPageUpdatedEventBinded = false;
    private bool isActiveLoadoutRemovedEventBinded = false;

    private void OnEnable()
    {
        if (!isInitializeLoadoutEventBinded)
        {
            BindOnInitializeLoadoutEvent();
        }

        if (!isLoadoutPageUpdatedEventBinded)
        {
            BindOnLoadoutPageUpdatedEvent();
        }

        if(!isActiveLoadoutRemovedEventBinded)
        {
            BindOnActiveLoadoutRemoveEvent();
        }
    }

    private void OnDisable()
    {
        if (isInitializeLoadoutEventBinded)
        {
            UnbindOnInitializeLoadoutEvent();
        }

        if (isLoadoutPageUpdatedEventBinded)
        {
            UnbindOnLoadoutPageUpdatedEvent();
        }

        if (isActiveLoadoutRemovedEventBinded)
        {
            UnbindOnActiveLoadoutRemoveEvent();
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

        upgradeCardFactory = GetComponent<LoadoutCardFactory>();
    }

    private void Start()
    {
        //avoid racing condition
        if(!isInitializeLoadoutEventBinded)
        {
            BindOnInitializeLoadoutEvent();
        }

        if (!isLoadoutPageUpdatedEventBinded)
        {
            BindOnLoadoutPageUpdatedEvent();
        }

        if (!isActiveLoadoutRemovedEventBinded)
        {
            BindOnActiveLoadoutRemoveEvent();
        }
    }


    #region Public methods
    public void HandleOnCardSelected(LoadoutCardGOInfo cardInfo)
    {
        AddToActiveCardSlots(cardInfo.upgradeSO);
    }

    public void HandleOnInitializeLayout(LoadoutData _loadoutData)
    {
        cachedLoadoutData = _loadoutData;
        InitializeLayout();
    }

    public void HandleOnPageUpdated(int currentPageIndex)
    {
        loadoutPageIndex = currentPageIndex;
        UpdateCardSlots(loadoutPageIndex);
    }

    public void HandleOnCardEndInteract()
    {
        ArrangeCardsInArc();
    }

    public void HandleOnActiveLoadoutRemoved(LoadoutCardGOInfo removedCardInfo)
    {
        cachedLoadoutData.totalUpgrades.Add(removedCardInfo.upgradeSO);
        UpdateCardSlots(loadoutPageIndex);
    }

    #endregion

    #region Internal methods
    private void RequestLoadoutGO()
    {
        for(int i = 0; i < displayAmountPerPage; i++)
        {
            LoadoutCardCreationInfo creationInfo = new(spawnContainer);
            GameObject card = upgradeCardFactory.CreateUpgradeCard(creationInfo);
            LoadoutCardGO upgradeCard = card.GetComponent<LoadoutCardGO>();

            cardSlots.Add(upgradeCard);
        }

        //bind on click
        BindOnCardSelectedEvent(GetCardSlots());

        //bind on endinteract
        BindOnCardEndInteractEvent(GetCardSlots());
    }

    private void InitializeLayout()
    {
        //request creation of upgrade layout
        RequestLoadoutGO();

        //update the layout buttons on the first page
        UpdateCardSlots(0);

        //update the layout into an arc
        ArrangeCardsInArc();

        //foreach upgrade in cachedloadout (save), set them to active
        InitializeEquippedUpgrades();
    }

    private void InitializeEquippedUpgrades()
    {
        foreach (UpgradeDefinitionSO upgrades in cachedLoadoutData.currentEquippedUpgrades)
        {
            AddToActiveCardSlots(upgrades);
        }
    }

    private void ArrangeCardsInArc()
    {
        int cardCount = cardSlots.Count;

        // Calculate the starting angle
        float totalAngle = (cardCount - 1) * angleBetweenCards;
        float startAngle = -totalAngle / 2;

        for (int i = 0; i < cardCount; i++)
        {
            float angle = startAngle + i * angleBetweenCards;
            float radian = angle * Mathf.Deg2Rad;

            // Calculate the card's position
            float x = spawnContainer.position.x + Mathf.Cos(radian) * radiusFromCenter;
            float y = spawnContainer.position.y + Mathf.Sin(radian) * radiusFromCenter;

            //adjust position
            cardSlots[i].gameObject.transform.localPosition = new Vector3(x, y, 0);
            cardSlots[i].gameObject.transform.position += new Vector3(offsetPosition.x, offsetPosition.y, offsetPosition.z);

            //rotate cards outwards
            cardSlots[i].gameObject.transform.localRotation = Quaternion.Euler(0, 0, angle + 90);

            //set the sorting order based on creation
            cardSlots[i].cardSpriteRenderer.sortingOrder = i;
            cardSlots[i].lockedSpriteRenderer.sortingOrder = i + 1;
        }
    }

    private List<LoadoutCardGO> GetCardSlots()
    {
        return cardSlots;
    }

    private void UpdateCardSlots(int pageIndex)
    {
        int startIndex = pageIndex * displayAmountPerPage;
        int endIndex = Mathf.Min(startIndex + displayAmountPerPage, cachedLoadoutData.totalUpgrades.Count);

        for (int i = 0; i < cardSlots.Count; i++)
        {
            if(i < endIndex - startIndex)
            {
                //compare if unlockedupgrade contains the total upgrade
                var upgrade = cachedLoadoutData.totalUpgrades[startIndex + i];
                var isUnlocked = cachedLoadoutData.totalUnlockedUpgrades.Any(unlockedUpgrade => unlockedUpgrade.upgradeName == upgrade.upgradeName);

                cardSlots[i].cardSpriteRenderer.sprite = upgrade.upgradeSprite;
                cardSlots[i].InitialiseLoadoutGO(new LoadoutCardGOInfo(upgrade), !isUnlocked);
                cardSlots[i].gameObject.SetActive(true);
            }
            else
            {
                cardSlots[i].InitialiseLoadoutGO(new LoadoutCardGOInfo(null), true);
                cardSlots[i].gameObject.SetActive(false);
            }
        }
    }

    private void AddToActiveCardSlots(UpgradeDefinitionSO selectedUpgrade)
    {
        //update the activeloadoutlayout
        bool success = ActiveLoadoutLayoutManager.AddUpgrade(selectedUpgrade);

        if(success)
        {
            //remove from the current list and update the layout
            cachedLoadoutData.totalUpgrades.Remove(selectedUpgrade);
            UpdateCardSlots(loadoutPageIndex);

            //temp, updates the button state
            LoadoutPageManager.Instance.UpdateButtonState();

            //broadcast event to LoadoutManager
            OnLoadoutSelectedEvent?.Invoke(selectedUpgrade);
        }
    }
    #endregion

    #region Bind CardEndInteract Event
    public void BindOnCardEndInteractEvent(List<LoadoutCardGO> cardUIList)
    {
        foreach (var card in cardUIList)
        {
            card.OnCardEndInteractEvent += HandleOnCardEndInteract;
        }
    }

    public void UnbindOnCardEndInteractEvent(List<CardUI> cardUIList)
    {
        foreach (var card in cardUIList)
        {
            card.OnCardEndInteractEvent -= HandleOnCardEndInteract;
        }
    }

    #endregion

    #region Bind ActiveLoadoutRemove Event

    public void BindOnActiveLoadoutRemoveEvent()
    {
        if (ActiveLoadoutLayoutManager)
        {
            ActiveLoadoutLayoutManager.OnActiveLoadoutRemovedEvent += HandleOnActiveLoadoutRemoved;
            isActiveLoadoutRemovedEventBinded = true;
        }
    }

    public void UnbindOnActiveLoadoutRemoveEvent()
    {
        if(ActiveLoadoutLayoutManager)
        {
            ActiveLoadoutLayoutManager.OnActiveLoadoutRemovedEvent -= HandleOnActiveLoadoutRemoved;
            isActiveLoadoutRemovedEventBinded = false;
        }
    }

    #endregion

    #region Bind LoadoutCardSelected Event

    public void BindOnCardSelectedEvent(List<LoadoutCardGO> cardSlots)
    {
        foreach (var card in cardSlots)
        {
            card.OnCardSelectedEvent += HandleOnCardSelected;
        }
    }

    public void UnbindOnCardSelectedEvent(List<LoadoutCardGO> cardSlots)
    {
        foreach (var card in cardSlots)
        {
            card.OnCardSelectedEvent -= HandleOnCardSelected;
        }
    }


    #endregion

    #region Bind LoadoutPageUpdate Event
    public void BindOnLoadoutPageUpdatedEvent()
    {
        if(LoadoutPageManager.Instance != null)
        {
            LoadoutPageManager.Instance.OnLoadoutPageUpdatedEvent += HandleOnPageUpdated;
            isLoadoutPageUpdatedEventBinded = true;
        }
    }

    public void UnbindOnLoadoutPageUpdatedEvent()
    {
        if (LoadoutPageManager.Instance != null)
        {
            LoadoutPageManager.Instance.OnLoadoutPageUpdatedEvent -= HandleOnPageUpdated;
            isLoadoutPageUpdatedEventBinded = false;
        }
    }
    #endregion

    #region Bind LoadoutInitialize Event
    public void BindOnInitializeLoadoutEvent()
    {
        if(LoadoutManager.Instance != null)
        {
            LoadoutManager.Instance.OnInitializeLoadoutEvent += HandleOnInitializeLayout;
            isInitializeLoadoutEventBinded = true;
        }
    }

    public void UnbindOnInitializeLoadoutEvent()
    {
        if (LoadoutManager.Instance != null)
        {
            LoadoutManager.Instance.OnInitializeLoadoutEvent -= HandleOnInitializeLayout;
            isInitializeLoadoutEventBinded = false;
        }
    }
    #endregion
}
