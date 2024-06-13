using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandUIController : MonoBehaviour
{
    public List<CardUI> CardUIContainer = new List<CardUI>();

    [Header("Card Controller Configs")]
    public float angleOfCards = 0f;
    public Vector3 cardOffset = Vector3.zero;

    [Header("Card Allignment Configs")]
    public GameObject cardUIPrefab;
    public float gapBetweenCards = 10f;

    private float startingCardHeight;
    private float cardUIExtent;

    private void Awake()
    {
        if(!cardUIPrefab)
        {
            throw new AccessViolationException("Assign CardUI Prefab");
        }

        startingCardHeight = GetComponent<RectTransform>().rect.height / -2;
        cardUIExtent = (cardUIPrefab.GetComponent<Image>().preferredWidth / 2) - gapBetweenCards;        
    }

    private void Start()
    {
        ResetCardsOffset();
        Invoke(nameof(AdjustHand), 0.1f);
    }

    private void Update()
    {
        
    }

    public void AdjustHand() 
    {
        ResetCardsOffset();

        int totalCardCount = GetCardsInHand().Count;
        int halfOfTotalCards = totalCardCount / 2;
        bool isEvenNum = totalCardCount % 2 == 0;

        //angle calculation
        float anglePerCard = angleOfCards / totalCardCount;
        float startAngle = angleOfCards / 2f;

        //alignment calculation
        float startPositionX = isEvenNum ? (Screen.width / 2f) - ((halfOfTotalCards) * cardUIExtent) + (cardUIExtent/2) : (Screen.width / 2f) - (halfOfTotalCards * cardUIExtent);

        int runningCount = 0;
        float xPosition;
        foreach (var card in CardUIContainer)
        {
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

    #region Internal Functions
    private List<CardUI> GetCardsInHand()
    {
        //unbind before reseting
        UnbindOnCardEndDragDelegate(CardUIContainer);

        //resets then gets all child of CardUI
        CardUIContainer.Clear();
        gameObject.GetComponentsInChildren(CardUIContainer);

        //bind all cards in list
        BindOnCardEndDragDelegate(CardUIContainer);

        return CardUIContainer;
    }

    private void ResetCardsOffset()
    {
        //get the new set of cards to list
        GetCardsInHand();
        foreach (var card in CardUIContainer)
        {
            var cardRectTransform = card.GetComponent<RectTransform>();
            Vector3 originalCardPosition = new(cardRectTransform.anchoredPosition3D.x, startingCardHeight, cardRectTransform.anchoredPosition3D.z);

            //reset position and rotation
            cardRectTransform.anchoredPosition3D = originalCardPosition;
            card.transform.localEulerAngles = Vector3.zero;
        }
    }

    public void BindOnCardEndDragDelegate(List<CardUI> cardUIList)
    {
        foreach(var card in cardUIList)
        {
            card.OnCardEndDrag += AdjustHand;
        }
    }

    public void UnbindOnCardEndDragDelegate(List<CardUI> cardUIList)
    {
        foreach (var card in cardUIList)
        {
            card.OnCardEndDrag -= AdjustHand;
        }
    }
    #endregion
}
