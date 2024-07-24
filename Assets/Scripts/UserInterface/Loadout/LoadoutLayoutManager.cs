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

    public LoadoutData loadoutData;

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
    }


    #region Public methods
    public void HandleOnCardSelected(LoadoutCardGOInfo cardInfo)
    {
        //broadcast event to LoadoutManager
        OnLoadoutSelectedEvent?.Invoke(cardInfo.upgradeSO);

        UpdateActiveCardSlots(cardInfo.upgradeSO);
    }

    public void HandleOnInitializeLayout(LoadoutData _loadoutData)
    {
        loadoutData = _loadoutData;
        InitializeLayout();
    }

    public void HandleOnPageUpdated(int currentPageIndex)
    {
        loadoutPageIndex = currentPageIndex;
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

        BindOnCardSelectedEvent(GetCardSlots());
    }

    private void InitializeLayout()
    {
        //create the objects
        RequestLoadoutGO();

        //update their sprite
        UpdateCardSlots(0);

        //arrange them
        ArrangeCardsInArc();
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
        int endIndex = Mathf.Min(startIndex + displayAmountPerPage, loadoutData.totalUpgrades.Count);

        for (int i = 0; i < cardSlots.Count; i++)
        {
            if(i < endIndex - startIndex)
            {
                //compare if unlockedupgrade contains the total upgrade
                var upgrade = loadoutData.totalUpgrades[startIndex + i];
                var isUnlocked = loadoutData.totalUnlockedUpgrades.Any(unlockedUpgrade => unlockedUpgrade.upgradeName == upgrade.upgradeName);

                cardSlots[i].cardSpriteRenderer.sprite = upgrade.upgradeSprite;
                cardSlots[i].InitialiseLoadoutGO(upgrade, !isUnlocked);
                cardSlots[i].gameObject.SetActive(true);
            }
            else
            {
                cardSlots[i].InitialiseLoadoutGO(null, true);
                cardSlots[i].gameObject.SetActive(false);
            }
        }
    }

    private void UpdateActiveCardSlots(UpgradeDefinitionSO selectedUpgrade)
    {
        //update the activeloadoutlayout
        bool success = ActiveLoadoutLayoutManager.UpdateLayout(selectedUpgrade);

        if(success)
        {
            //remove from the current list and update the layout
            loadoutData.totalUpgrades.Remove(selectedUpgrade);
            UpdateCardSlots(loadoutPageIndex);
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
