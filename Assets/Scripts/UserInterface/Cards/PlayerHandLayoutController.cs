using System.Collections.Generic;
using GameCore;
using GameCore.TurnSystem;
using GameCore.TurnSystem.Phases.Base;
using UnityEngine;
using PlayerCore;
using PlayerCore.AIPlayer;
using UserInterface.Cards.Base;
using UserInterface.Cards.ChoiceCard;

namespace UserInterface.Cards
{
    [RequireComponent(typeof(ChoiceCardUIFactory))]
    public class PlayerHandLayoutController : MonoBehaviour
    {
        public List<CardUI> cardUIContainer = new List<CardUI>();

        [Header("Card Allignment Configs")] 
        public Transform spawnContainer;

        [Tooltip("How much each card rotates")]
        public float totalAngleOfHand = 0f;

        [Tooltip("How much to offset the cards on the side (left & right)")]
        public float cardOffsetYDuringRotation;

        [Tooltip("Offset in between each card")]
        public float gapBetweenCards = 10f;

        [Header("Card Controller Configs")] 
        public Player attachedPlayer;

        //cached values
        private RectTransform spawnRectTransform;
        private float startingCardHeight;
        private float cardUIPrefabExtent;

        //event binding flags
        private bool isOnClearHandEventBinded = false;
        private bool isOnSealChoiceEventBinded = false;
        private bool isOnPlayerPhaseEventBinded = false;

        //factory
        private ChoiceCardUIFactory choiceCardFactory;

        private void OnDisable()
        {
            if (isOnClearHandEventBinded)
            {
                UnbindClearCardHandEvent();
            }

            if (isOnSealChoiceEventBinded)
            {
                UnbindSealChoiceEvent();
            }

            if (isOnPlayerPhaseEventBinded)
            {
                UnbindOnPlayerPhaseStart();
            }
        }

        private void Awake()
        {
            choiceCardFactory = GetComponent<ChoiceCardUIFactory>();

            //caching values
            spawnRectTransform = spawnContainer.GetComponent<RectTransform>();
            startingCardHeight = spawnRectTransform.rect.height / -2; //based on pivot points
            cardUIPrefabExtent = (choiceCardFactory.choiceCardUIPrefab.GetComponent<RectTransform>().rect.width / 2) +
                                 gapBetweenCards;
        }

        private void Start()
        {
            if (!isOnClearHandEventBinded)
            {
                BindClearCardHandEvent();
            }

            if (!isOnSealChoiceEventBinded)
            {
                BindSealChoiceEvent();
            }

            if (!isOnPlayerPhaseEventBinded)
            {
                BindOnPlayerPhaseStart();
            }
        }

        #region public methods

        public void HandleOnOnPlayerStartPhase(Player player)
        {
            //used to deal/display the card choices.
            RequestCardChoices(attachedPlayer);
        }

        public void HandleOnClearCardHand()
        {
            ClearCardsInHand();
        }

        public void HandleOnCardEndInteract()
        {
            AdjustHand();
        }

        public void HandleOnSealChoice()
        {
            RequestCardChoices(attachedPlayer);
        }

        #endregion

        #region Internal Functions

        private void ClearCardsInHand()
        {
            // get the current cards in hand
            var cardsInHand = GetCardsInHand();

            //unbind before reseting
            UnbindOnCardEndInteractDelegate(cardsInHand);

            //resets then gets all child of CardUI
            foreach (var card in cardsInHand)
            {
                Destroy(card.gameObject);
            }

            cardUIContainer.Clear();
        }

        private void RequestCardChoices(Player player)
        {
            //every request, clear the cards in hand
            ClearCardsInHand();

            //create based on choices available
            foreach (var choice in player.ChoiceComponent.choicesAvailable)
            {
                FChoiceCardCreation creation = new(choice.Key, spawnContainer);
                GameObject cardUIGO = choiceCardFactory.CreateCard(creation);
                ChoiceCardUI cardUI = cardUIGO.GetComponent<ChoiceCardUI>();

                //Initialise the UI's values
                if (player.GetComponent<AIPlayer>())
                {
                    cardUI.InitialiseCard(choice.Key, choice.Value, isInteractable: false);
                }
                else
                {
                    cardUI.InitialiseCard(choice.Key, choice.Value, isInteractable: true);
                }

                //add to list, set to inactive and wait for AdjustHand
                cardUIContainer.Add(cardUI);
                cardUIGO.SetActive(false);
            }

            BindOnCardEndInteractEvent(GetCardsInHand());
            AdjustHand();
        }

        private void AdjustHand()
        {
            ResetCardsOffset();

            var cardsInHand = GetCardsInHand();
            int totalCardCount = cardsInHand.Count;
            int halfOfTotalCards = totalCardCount / 2;
            bool isEvenNum = totalCardCount % 2 == 0;

            //card angle calculation
            float anglePerCard = totalAngleOfHand / totalCardCount;
            float startAngleOfHand = totalAngleOfHand / 2f;

            //alignment calculation
            float startPositionX = isEvenNum
                ? (spawnRectTransform.rect.width / 2f) - ((halfOfTotalCards) * cardUIPrefabExtent) +
                  (cardUIPrefabExtent / 2)
                : (spawnRectTransform.rect.width / 2f) - (halfOfTotalCards * cardUIPrefabExtent);

            int runningCount = 0;
            float xPosition;
            foreach (var card in cardsInHand)
            {
                var cardRectTransform = card.GetComponent<RectTransform>();

                float currentAngle = startAngleOfHand - (anglePerCard * runningCount);

                //we have to increment by 1 to skip the center.
                if (runningCount > halfOfTotalCards)
                {
                    currentAngle = startAngleOfHand - (anglePerCard * (runningCount + 1));
                }

                // Calculate x position relative to the size of the card
                xPosition = startPositionX + (runningCount * cardUIPrefabExtent);

                // Calculate y position and rotation, add offset based on which ever half you are on (left half, right half)
                float yPosition = startingCardHeight +
                                  (cardOffsetYDuringRotation * Mathf.Abs(halfOfTotalCards - runningCount));

                //if you are odd, or you are at the half way point, then do not rotate (0f)
                float rotationAngle = (isEvenNum || runningCount != halfOfTotalCards) ? currentAngle : 0f;

                // Apply rotation and position
                cardRectTransform.localEulerAngles = new Vector3(0f, 0f, rotationAngle);
                cardRectTransform.anchoredPosition3D =
                    new Vector3(xPosition, yPosition, cardRectTransform.anchoredPosition3D.z);

                runningCount++;
                card.gameObject.SetActive(true);
            }
        }

        private List<CardUI> GetCardsInHand()
        {
            return cardUIContainer;
        }

        private void ResetCardsOffset()
        {
            //get the new set of cards to list
            var cardsInHand = GetCardsInHand();
            foreach (var card in cardsInHand)
            {
                var cardRectTransform = card.GetComponent<RectTransform>();
                Vector3 originalCardPosition = new(cardRectTransform.anchoredPosition3D.x, startingCardHeight,
                    cardRectTransform.anchoredPosition3D.z);

                //reset position and rotation
                cardRectTransform.anchoredPosition3D = originalCardPosition;
                card.transform.localEulerAngles = Vector3.zero;
            }
        }

        #endregion

        #region Bind CardEndDrag Delegate

        public void BindOnCardEndInteractEvent(List<CardUI> cardUIList)
        {
            foreach (var card in cardUIList)
            {
                card.OnCardInteractEnd += HandleOnCardEndInteract;
            }
        }

        public void UnbindOnCardEndInteractDelegate(List<CardUI> cardUIList)
        {
            foreach (var card in cardUIList)
            {
                card.OnCardInteractEnd -= HandleOnCardEndInteract;
            }
        }

        #endregion

        #region Bind OnPlayerPhaseStart

        private void BindOnPlayerPhaseStart()
        {
            if (TurnSystemManager.Instance != null)
            {
                TurnSystemManager.Instance.PlayerPhase.OnPlayerPhaseStart += HandleOnOnPlayerStartPhase;
                isOnPlayerPhaseEventBinded = true;
            }
        }

        private void UnbindOnPlayerPhaseStart()
        {
            if (TurnSystemManager.Instance != null)
            {
                TurnSystemManager.Instance.PlayerPhase.OnPlayerPhaseStart -= HandleOnOnPlayerStartPhase;
                isOnPlayerPhaseEventBinded = false;
            }
        }

        #endregion

        #region Bind ClearCardHand Delegate

        private void BindClearCardHandEvent()
        {
            if (TurnSystemManager.Instance != null)
            {
                TurnSystemManager.Instance.EvaluationPhase.OnClearCardInHand += HandleOnClearCardHand;
                isOnClearHandEventBinded = true;
            }
        }

        private void UnbindClearCardHandEvent()
        {
            if (TurnSystemManager.Instance != null)
            {
                TurnSystemManager.Instance.EvaluationPhase.OnClearCardInHand -= HandleOnClearCardHand;
                isOnClearHandEventBinded = false;
            }
        }

        #endregion

        #region Bind SealChoice Delegate

        private void BindSealChoiceEvent()
        {
            if (attachedPlayer && attachedPlayer.ChoiceComponent)
            {
                attachedPlayer.ChoiceComponent.OnChoiceSealed += HandleOnSealChoice;
                isOnSealChoiceEventBinded = true;
            }
        }

        private void UnbindSealChoiceEvent()
        {
            if (attachedPlayer && attachedPlayer.ChoiceComponent)
            {
                attachedPlayer.ChoiceComponent.OnChoiceSealed += HandleOnSealChoice;
                isOnSealChoiceEventBinded = false;
            }
        }

        #endregion
    }
}