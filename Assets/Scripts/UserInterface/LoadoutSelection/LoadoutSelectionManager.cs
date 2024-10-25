using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.LoadoutSelection;
using GameCore.LoadoutSelection.LoadoutCardObj;
using UnityEngine;
using PlayerCore.Upgrades.Base;
using UserInterface.Cards.Base;

namespace UserInterface.LoadoutSelection
{
    [RequireComponent(typeof(LoadoutCardFactory))]
    public class LoadoutSelectionManager : MonoBehaviour
    {
        public static LoadoutSelectionManager Instance;

        [Header("Dependencies")] 
        public EquippedLoadoutManager equippedLoadoutManager;

        [Header("Layout Config")] 
        public Transform spawnContainer;
        public float radiusFromCenter;
        public float angleBetweenCards;
        [Range(1, 9)] public int displayAmountPerPage = 9;

        [Header("Layout Offset Config")] 
        [Tooltip("This value is relative to World Position")] public Vector3 offsetPosition;

        public FLoadoutData cachedLoadoutData;

        //private
        private List<LoadoutCardObj> cardDisplayList = new();
        private int loadoutPageIndex;

        //factory
        private LoadoutCardFactory upgradeCardFactory;

        //event
        public event Action<UpgradeDefinitionSO> OnLoadoutSelected;
        public event Action<UpgradeDefinitionSO> OnEquippedLoadoutRemoved;

        //flag
        private bool isInitializeLoadoutEventBinded;
        private bool isLoadoutPageUpdatedEventBinded;
        private bool isActiveLoadoutRemovedEventBinded;

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

            if (!isActiveLoadoutRemovedEventBinded)
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
            if (Instance != null && Instance != this)
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
            if (!isInitializeLoadoutEventBinded)
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

        private void HandleOnCardSelected(FLoadoutCardObj cardInfo)
        {
            AddToActiveCardSlots(cardInfo.upgradeSO);
        }

        private void HandleOnInitializeLayout(FLoadoutData fLoadoutData)
        {
            cachedLoadoutData = fLoadoutData;
            InitializeLayout();
        }

        private void HandleOnPageUpdated(int currentPageIndex)
        {
            loadoutPageIndex = currentPageIndex;
            UpdateCardDisplay(loadoutPageIndex);
        }

        private void HandleOnCardEndInteract()
        {
            ArrangeCardsInArc();
        }

        private void HandleOnActiveLoadoutRemoved(FLoadoutCardObj removedCardInfo)
        {
            //broadcast event to LoadoutManager to update the data (remove and add from total upgrades and equipped)
            OnEquippedLoadoutRemoved?.Invoke(removedCardInfo.upgradeSO);

            UpdateCardDisplay(loadoutPageIndex);
        }

        #endregion

        #region Internal methods

        private void RequestLoadoutObj()
        {
            for (int i = 0; i < displayAmountPerPage; i++)
            {
                FLoadoutCardCreation creation = new(spawnContainer);
                GameObject card = upgradeCardFactory.CreateUpgradeCard(creation);
                LoadoutCardObj upgradeCard = card.GetComponent<LoadoutCardObj>();

                cardDisplayList.Add(upgradeCard);
            }

            //bind on click
            BindOnCardSelectedEvent(GetCardSlots());

            //bind on end interact
            BindOnCardEndInteractEvent(GetCardSlots());
        }

        private void InitializeLayout()
        {
            //request creation of upgrade layout
            RequestLoadoutObj();

            //update the layout into an arc
            ArrangeCardsInArc();

            //update the layout buttons on the first page
            UpdateCardDisplay(0);

            LoadoutSelectionPageManager.Instance.UpdateButtonState();

            InitializeEquippedUpgrades();
        }

        private void InitializeEquippedUpgrades()
        {
            var upgradesCopy = new List<UpgradeDefinitionSO>(cachedLoadoutData.totalEquippedUpgrades);

            foreach (var upgrades in upgradesCopy)
            {
                AddToActiveCardSlots(upgrades);
            }
        }

        private void ArrangeCardsInArc()
        {
            int cardCount = cardDisplayList.Count;

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
                cardDisplayList[i].gameObject.transform.localPosition = new Vector3(x, y, 0);
                cardDisplayList[i].gameObject.transform.position +=
                    new Vector3(offsetPosition.x, offsetPosition.y, offsetPosition.z);

                //rotate cards outwards
                cardDisplayList[i].gameObject.transform.localRotation = Quaternion.Euler(0, 0, angle + 90);

                //set the sorting order based on creation
                cardDisplayList[i].cardSpriteRenderer.sortingOrder = i;
                cardDisplayList[i].lockedSpriteRenderer.sortingOrder = i + 1;
            }
        }

        private List<LoadoutCardObj> GetCardSlots()
        {
            return cardDisplayList;
        }

        private void UpdateCardDisplay(int pageIndex)
        {
            int startIndex = pageIndex * displayAmountPerPage;
            int endIndex = Mathf.Min(startIndex + displayAmountPerPage, cachedLoadoutData.totalUpgradesInGame.Count);

            for (int i = 0; i < cardDisplayList.Count; i++)
            {
                if (i < endIndex - startIndex)
                {
                    //compare if unlocked upgrade contains the total upgrade
                    var upgrade = cachedLoadoutData.totalUpgradesInGame[startIndex + i];
                    var isUnlocked = cachedLoadoutData.totalUnlockedUpgrades.Any(unlockedUpgrade =>
                        unlockedUpgrade.upgradeName == upgrade.upgradeName);

                    cardDisplayList[i].cardSpriteRenderer.sprite = upgrade.upgradeSprite;
                    cardDisplayList[i].InitialiseLoadoutGO(new FLoadoutCardObj(upgrade), !isUnlocked);
                    cardDisplayList[i].gameObject.SetActive(true);
                }
                else
                {
                    cardDisplayList[i].InitialiseLoadoutGO(new FLoadoutCardObj(null), true);
                    cardDisplayList[i].gameObject.SetActive(false);
                }
            }
        }

        private void AddToActiveCardSlots(UpgradeDefinitionSO selectedUpgrade)
        {
            //update the activeloadoutlayout
            bool success = equippedLoadoutManager.AddUpgrade(selectedUpgrade);

            if (success)
            {
                //broadcast event to LoadoutManager to update the data (remove and add from total upgrades and equipped)
                OnLoadoutSelected?.Invoke(selectedUpgrade);

                UpdateCardDisplay(loadoutPageIndex);

                //temp, updates the button state
                LoadoutSelectionPageManager.Instance.UpdateButtonState();
            }
        }

        #endregion

        #region Bind CardEndInteract Event

        public void BindOnCardEndInteractEvent(List<LoadoutCardObj> cardUIList)
        {
            foreach (var card in cardUIList)
            {
                card.OnCardInteractEnd += HandleOnCardEndInteract;
            }
        }

        public void UnbindOnCardEndInteractEvent(List<CardUI> cardUIList)
        {
            foreach (var card in cardUIList)
            {
                card.OnCardInteractEnd -= HandleOnCardEndInteract;
            }
        }

        #endregion

        #region Bind ActiveLoadoutRemove Event

        public void BindOnActiveLoadoutRemoveEvent()
        {
            if (equippedLoadoutManager)
            {
                equippedLoadoutManager.OnLoadoutRemoved += HandleOnActiveLoadoutRemoved;
                isActiveLoadoutRemovedEventBinded = true;
            }
        }

        public void UnbindOnActiveLoadoutRemoveEvent()
        {
            if (equippedLoadoutManager)
            {
                equippedLoadoutManager.OnLoadoutRemoved -= HandleOnActiveLoadoutRemoved;
                isActiveLoadoutRemovedEventBinded = false;
            }
        }

        #endregion

        #region Bind LoadoutCardSelected Event

        public void BindOnCardSelectedEvent(List<LoadoutCardObj> activeCards)
        {
            foreach (var card in activeCards)
            {
                card.OnCardSelected += HandleOnCardSelected;
            }
        }

        public void UnbindOnCardSelectedEvent(List<LoadoutCardObj> activeCards)
        {
            foreach (var card in activeCards)
            {
                card.OnCardSelected -= HandleOnCardSelected;
            }
        }

        #endregion

        #region Bind LoadoutPageUpdate Event

        public void BindOnLoadoutPageUpdatedEvent()
        {
            if (LoadoutSelectionPageManager.Instance != null)
            {
                LoadoutSelectionPageManager.Instance.OnLoadoutPageUpdated += HandleOnPageUpdated;
                isLoadoutPageUpdatedEventBinded = true;
            }
        }

        public void UnbindOnLoadoutPageUpdatedEvent()
        {
            if (LoadoutSelectionPageManager.Instance != null)
            {
                LoadoutSelectionPageManager.Instance.OnLoadoutPageUpdated -= HandleOnPageUpdated;
                isLoadoutPageUpdatedEventBinded = false;
            }
        }

        #endregion

        #region Bind LoadoutInitialize Event

        public void BindOnInitializeLoadoutEvent()
        {
            if (LoadoutManager.Instance != null)
            {
                LoadoutManager.Instance.OnInitializeLoadout += HandleOnInitializeLayout;
                isInitializeLoadoutEventBinded = true;
            }
        }

        public void UnbindOnInitializeLoadoutEvent()
        {
            if (LoadoutManager.Instance != null)
            {
                LoadoutManager.Instance.OnInitializeLoadout -= HandleOnInitializeLayout;
                isInitializeLoadoutEventBinded = false;
            }
        }

        #endregion
    }
}