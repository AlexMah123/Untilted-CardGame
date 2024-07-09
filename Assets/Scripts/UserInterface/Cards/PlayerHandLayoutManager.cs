using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ChoiceCardUIFactory))]
public class PlayerHandLayoutManager : MonoBehaviour
{
    public List<CardUI> CardUIContainer = new List<CardUI>();

    [Header("Card Allignment Configs")]
    public GameObject cardUIPrefab;
    [Tooltip("How much each card rotates")] public float totalAngleOfHand = 0f;
    [Tooltip("How much to offset the cards on the side (left & right)")] public float cardOffsetYDuringRotation;
    [Tooltip("Offset in between each card")] public float gapBetweenCards = 10f;

    [Header("Card Controller Configs")]
    public Player attachedPlayer;

    //cached values
    private RectTransform rectTransform;
    private float startingCardHeight;
    private float cardUIPrefabExtent;

    //event binding flags
    private bool isOnClearHandEventBinded = false;
    private bool isOnChangeTurnEventBinded = false;
    private bool isOnSealChoiceEventBinded = false;

    //factory
    private ChoiceCardUIFactory choiceCardFactory;

    private void OnEnable()
    {
        if(TurnSystemManager.Instance != null)
        {
            BindChangeTurnEvent();
        }

        if(GameManager.Instance != null)
        {
            BindClearCardHandEvent();
        }

        if(attachedPlayer != null)
        {
            BindSealChoiceEvent();
        }
    }

    private void OnDisable()
    {
        if (isOnChangeTurnEventBinded)
        {
            UnbindChangeTurnEvent();
        }

        if(isOnClearHandEventBinded)
        {
            UnbindClearCardHandEvent();
        }

        if(isOnSealChoiceEventBinded)
        {
            UnbindSealChoiceEvent();
        }
    }

    private void Awake()
    {
        if (!cardUIPrefab)
        {
            throw new MissingComponentException("CardUI Prefab is not assigned");
        }

        //caching values
        rectTransform = GetComponent<RectTransform>();
        startingCardHeight = rectTransform.rect.height / -2; //based on pivot points
        cardUIPrefabExtent = (cardUIPrefab.GetComponent<RectTransform>().rect.width / 2) + gapBetweenCards;

        choiceCardFactory = GetComponent<ChoiceCardUIFactory>();
    }


    private void Start()
    {
        //To avoid racing condition
        if (!isOnClearHandEventBinded)
        {
            BindClearCardHandEvent();
        }

        if(!isOnChangeTurnEventBinded)
        {
            BindChangeTurnEvent();
        }

        if(!isOnSealChoiceEventBinded)
        {
            BindSealChoiceEvent();
        }
    }

    #region public methods

    public void HandleOnPlayerStartTurn(Player player)
    {
        RequestCardChoices(attachedPlayer);
    }

    public void HandleOnClearCardHand()
    {
        ClearCardsInHand();
    }

    public void HandleOnCardEndDrag()
    {
        AdjustHand();
    }

    public void HandleOnChangedTurn(TurnSystemManager manager, Turn currentTurn, Turn newTurn)
    {
        if (currentTurn != null)
        {
            currentTurn.OnPlayerStartTurnEvent -= HandleOnPlayerStartTurn;
        }

        newTurn.OnPlayerStartTurnEvent += HandleOnPlayerStartTurn;
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
        UnbindOnCardEndDragDelegate(cardsInHand);

        //resets then gets all child of CardUI
        foreach (var card in cardsInHand)
        {
            Destroy(card.gameObject);
        }

        CardUIContainer.Clear();
    }

    private void RequestCardChoices(Player player)
    {
        //every request, clear the cards in hand
        ClearCardsInHand();

        //create based on choices available
        foreach (var choice in player.ChoiceComponent.choicesAvailable)
        {
            GameObject cardUIGO = choiceCardFactory.CreateCard(choice.Key, transform);
            ChoiceCardUI cardUI = cardUIGO.GetComponent<ChoiceCardUI>();

            //Initiailise the UI's values
            if(player.GetComponent<AIPlayer>())
            {
                cardUI.InitialiseCard(choice.Key, choice.Value, isInteractable: false);
            }
            else
            {
                cardUI.InitialiseCard(choice.Key, choice.Value, isInteractable: true);
            }

            //add to list, set to inactive and wait for adjusthand
            CardUIContainer.Add(cardUI);
            cardUIGO.SetActive(false);
        }

        BindOnCardEndDragEvent(GetCardsInHand());
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
            ? (rectTransform.rect.width / 2f) - ((halfOfTotalCards) * cardUIPrefabExtent) + (cardUIPrefabExtent / 2) 
            : (rectTransform.rect.width / 2f) - (halfOfTotalCards * cardUIPrefabExtent);

        int runningCount = 0;
        float xPosition;
        foreach (var card in cardsInHand)
        {
            var cardRectTransform = card.GetComponent<RectTransform>();

            float currentAngle = startAngleOfHand - (anglePerCard * runningCount);

            //we have to increment by 1 to skip the center.
            if(runningCount > halfOfTotalCards)
            {
                currentAngle = startAngleOfHand - (anglePerCard * (runningCount + 1));
            }

            // Calculate x position relative to the size of the card
            xPosition = startPositionX + (runningCount * cardUIPrefabExtent);

            // Calculate y position and rotation, add offset based on which ever half you are on (left half, right half)
            float yPosition = startingCardHeight + (cardOffsetYDuringRotation * Mathf.Abs(halfOfTotalCards - runningCount));

            //if you are odd, or you are at the half way point, then do not rotate (0f)
            float rotationAngle = (isEvenNum || runningCount != halfOfTotalCards) ? currentAngle : 0f;

            // Apply rotation and position
            cardRectTransform.localEulerAngles = new Vector3(0f, 0f, rotationAngle);
            cardRectTransform.anchoredPosition3D = new Vector3(xPosition, yPosition, cardRectTransform.anchoredPosition3D.z);

            runningCount++;
            card.gameObject.SetActive(true);
        }
    }

    private List<CardUI> GetCardsInHand()
    {
        return CardUIContainer;
    }

    private void ResetCardsOffset()
    {
        //get the new set of cards to list
        var cardsInHand = GetCardsInHand();
        foreach (var card in cardsInHand)
        {
            var cardRectTransform = card.GetComponent<RectTransform>();
            Vector3 originalCardPosition = new(cardRectTransform.anchoredPosition3D.x, startingCardHeight, cardRectTransform.anchoredPosition3D.z);

            //reset position and rotation
            cardRectTransform.anchoredPosition3D = originalCardPosition;
            card.transform.localEulerAngles = Vector3.zero;
        }
    }


    #endregion

    #region Bind CardEndDrag Delegate
    public void BindOnCardEndDragEvent(List<CardUI> cardUIList)
    {
        foreach (var card in cardUIList)
        {
            card.OnCardEndDragEvent += HandleOnCardEndDrag;
        }
    }

    public void UnbindOnCardEndDragDelegate(List<CardUI> cardUIList)
    {
        foreach (var card in cardUIList)
        {
            card.OnCardEndDragEvent -= HandleOnCardEndDrag;
        }
    }

    #endregion

    #region Bind ClearCardHand Delegate

    private void BindClearCardHandEvent()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnClearCardHandEvent += HandleOnClearCardHand;
            isOnClearHandEventBinded = true;
        }
    }

    private void UnbindClearCardHandEvent()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnClearCardHandEvent -= HandleOnClearCardHand;
            isOnClearHandEventBinded = false;
        }

    }

    #endregion

    #region Bind ChangeTurn Delegate

    private void BindChangeTurnEvent()
    {
        if (TurnSystemManager.Instance != null)
        {
            TurnSystemManager.Instance.OnChangedTurnEvent += HandleOnChangedTurn;
            isOnChangeTurnEventBinded = true;
        }

    }

    private void UnbindChangeTurnEvent()
    {
        if (TurnSystemManager.Instance != null)
        {
            TurnSystemManager.Instance.OnChangedTurnEvent -= HandleOnChangedTurn;
            isOnChangeTurnEventBinded = false;
        }
    }
    #endregion

    #region Bind SealChoice Delegate
    private void BindSealChoiceEvent()
    {
        if(attachedPlayer && attachedPlayer.ChoiceComponent)
        {
            attachedPlayer.ChoiceComponent.OnSealChoiceEvent += HandleOnSealChoice;
            isOnSealChoiceEventBinded = true;
        }
    }

    private void UnbindSealChoiceEvent()
    {
        if (attachedPlayer && attachedPlayer.ChoiceComponent)
        {
            attachedPlayer.ChoiceComponent.OnSealChoiceEvent += HandleOnSealChoice;
            isOnSealChoiceEventBinded = false;
        }
    }

    #endregion
}
