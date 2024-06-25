using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandUIManager : MonoBehaviour
{
    public static PlayerHandUIManager Instance;

    public List<CardUI> CardUIContainer = new List<CardUI>();

    [Header("Card Controller Configs")]
    public float angleOfCards = 0f;
    public Vector3 cardOffset = Vector3.zero;
    public int cardAmount;

    [Header("Card Allignment Configs")]
    public GameObject cardUIPrefab;
    public float gapBetweenCards = 10f;

    //cached values
    private float startingCardHeight;
    private float cardUIExtent;
    private bool isOnClearHandEventBinded = false;

    private void OnEnable()
    {
        TurnSystemManager.Instance.OnChangedTurnEvent += HandleOnChangedTurn;

        if(GameManager.Instance != null)
        {
            BindClearCardHandEvent();
        }
    }

    private void OnDisable()
    {
        TurnSystemManager.Instance.OnChangedTurnEvent -= HandleOnChangedTurn;

        if(isOnClearHandEventBinded)
        {
            UnbindClearCardHandEvent();

        }
    }

    private void Awake()
    {
        //singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        if (!cardUIPrefab)
        {
            throw new MissingComponentException("CardUI Prefab is not assigned");
        }

        //caching values
        startingCardHeight = GetComponent<RectTransform>().rect.height / -3;
        cardUIExtent = (cardUIPrefab.GetComponent<Image>().preferredWidth / 2) - gapBetweenCards;
    }


    private void Start()
    {
        //To avoid racing condition
        if (!isOnClearHandEventBinded)
        {
            BindClearCardHandEvent();
        }
    }

    #region public methods

    public void HandleOnPlayerStartTurn(IPlayer player)
    {
        RequestCardChoices(player);
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
        //Debug.Log($"Changing Player Turn - From: {currentTurn}, To: {newTurn}");

        if (currentTurn != null)
        {
            currentTurn.OnPlayerStartTurnEvent -= HandleOnPlayerStartTurn;
        }

        newTurn.OnPlayerStartTurnEvent += HandleOnPlayerStartTurn;
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

    private void RequestCardChoices(IPlayer player)
    {
        //every request, clear the cards in hand
        ClearCardsInHand();

        //create based on choices available
        foreach (var choice in player.ChoiceComponent.choicesAvailable)
        {
            GameObject cardUIGO = Instantiate(cardUIPrefab, transform);
            CardUI cardUI = cardUIGO.GetComponent<CardUI>();

            //Initiailise the UI's values
            cardUI.InitialiseCard(choice.Key, choice.Value);

            //add to list, set to inactive and wait for adjusthand
            CardUIContainer.Add(cardUI);
            cardUIGO.SetActive(false);
        }

        BindOnCardEndDragDelegate(GetCardsInHand());
        AdjustHand();
    }

    private void AdjustHand()
    {
        ResetCardsOffset();

        var cardsInHand = GetCardsInHand();
        int totalCardCount = cardsInHand.Count;
        int halfOfTotalCards = totalCardCount / 2;
        bool isEvenNum = totalCardCount % 2 == 0;

        //angle calculation
        float anglePerCard = angleOfCards / totalCardCount;
        float startAngle = angleOfCards / 2f;

        //alignment calculation
        float startPositionX = isEvenNum ? (Screen.width / 2f) - ((halfOfTotalCards) * cardUIExtent) + (cardUIExtent / 2) : (Screen.width / 2f) - (halfOfTotalCards * cardUIExtent);

        int runningCount = 0;
        float xPosition;
        foreach (var card in cardsInHand)
        {
            card.gameObject.SetActive(true);

            var cardRectTransform = card.GetComponent<RectTransform>();

            if (runningCount == halfOfTotalCards)
            {
                //since half of the hand count be decimal, increment to round it off.
                runningCount++;

                xPosition = startPositionX + ((runningCount - 1) * cardUIExtent);

                //if it is odd, we do not have to rotate
                if (isEvenNum)
                {
                    card.transform.Rotate(0f, 0f, startAngle - (anglePerCard * runningCount));
                    cardRectTransform.anchoredPosition3D = new Vector3(xPosition, startingCardHeight + (cardOffset.y * (runningCount - halfOfTotalCards)), cardRectTransform.anchoredPosition3D.z);
                }
                else
                {
                    card.transform.localEulerAngles = Vector3.zero;
                    cardRectTransform.anchoredPosition3D = new Vector3(xPosition, startingCardHeight + (cardOffset.y / 2), cardRectTransform.anchoredPosition3D.z);
                }
            }
            else
            {
                card.transform.Rotate(0f, 0f, startAngle - (anglePerCard * runningCount));

                //left side of hand
                if (runningCount < halfOfTotalCards)
                {
                    cardRectTransform.anchoredPosition3D = new Vector3(startPositionX + (runningCount * cardUIExtent), startingCardHeight + (cardOffset.y * (halfOfTotalCards - runningCount)), cardRectTransform.anchoredPosition3D.z);
                }
                else
                {
                    xPosition = startPositionX + ((runningCount - 1) * cardUIExtent);

                    //right side of hand
                    //if it is odd, the center card will not be offsetted, hence minusing 1 manually by 1 (skipping it)
                    if (isEvenNum)
                    {
                        cardRectTransform.anchoredPosition3D = new Vector3(xPosition, startingCardHeight + (cardOffset.y * (runningCount - halfOfTotalCards)), cardRectTransform.anchoredPosition3D.z);
                    }
                    else
                    {
                        cardRectTransform.anchoredPosition3D = new Vector3(xPosition, startingCardHeight + (cardOffset.y * (runningCount - halfOfTotalCards - 1)), cardRectTransform.anchoredPosition3D.z);
                    }

                }
            }

            runningCount++;
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

    #region Bind Delegate
    public void BindOnCardEndDragDelegate(List<CardUI> cardUIList)
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
}
