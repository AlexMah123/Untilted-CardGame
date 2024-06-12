using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerHandUIController : MonoBehaviour
{
    public List<CardUI> CardUIContainer = new List<CardUI>();

    [Header("Card Controller Defaults")]
    public float angleOfCards = 0f;
    public Vector3 cardOffset = Vector3.zero;

    private float startingCardHeight;

    private void Awake()
    {
        startingCardHeight = GetComponent<RectTransform>().rect.height / -2;

        ResetCardsOffset();
        Invoke(nameof(AdjustHand), 0.1f);
    }

    private void Start()
    {
        
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

        int runningCount = 0;
        foreach (var card in CardUIContainer)
        {
            var cardRectTransform = card.GetComponent<RectTransform>();

            if (runningCount == halfOfTotalCards)
            {
                //since the center has custom calculations. forcibly increment so the left and right halfs are equal
                runningCount++;

                if (isEvenNum)
                {
                    card.transform.Rotate(0f, 0f, startAngle - (anglePerCard * runningCount));
                    cardRectTransform.anchoredPosition3D = new Vector3(cardRectTransform.anchoredPosition3D.x, cardRectTransform.anchoredPosition3D.y + cardOffset.y * (runningCount - halfOfTotalCards), cardRectTransform.anchoredPosition3D.z);
                }
                else
                {
                    card.transform.localEulerAngles = Vector3.zero;
                }
            }
            else
            {
                card.transform.Rotate(0f, 0f, startAngle - (anglePerCard * runningCount));

                //left side of hand
                if(runningCount < halfOfTotalCards)
                {
                    cardRectTransform.anchoredPosition3D = new Vector3(cardRectTransform.anchoredPosition3D.x, cardRectTransform.anchoredPosition3D.y + cardOffset.y * (halfOfTotalCards - runningCount), cardRectTransform.anchoredPosition3D.z);
                }
                else
                {
                    //right side of hand
                    //if it is odd, the center card will not be offsetted, hence incrementing manually by 1 (skipping it)
                    if(isEvenNum)
                    {
                        cardRectTransform.anchoredPosition3D = new Vector3(cardRectTransform.anchoredPosition3D.x, cardRectTransform.anchoredPosition3D.y + cardOffset.y * (runningCount - halfOfTotalCards), cardRectTransform.anchoredPosition3D.z);
                    }
                    else
                    {
                        cardRectTransform.anchoredPosition3D = new Vector3(cardRectTransform.anchoredPosition3D.x, cardRectTransform.anchoredPosition3D.y + cardOffset.y * (runningCount - halfOfTotalCards - 1), cardRectTransform.anchoredPosition3D.z);
                    }
                    
                }
            }

            runningCount++;
        }
    }

    #region Internal Functions
    private List<CardUI> GetCardsInHand()
    {
        //resets then gets all child of CardUI
        CardUIContainer.Clear();
        gameObject.GetComponentsInChildren(CardUIContainer);

        return CardUIContainer;
    }

    private void ResetCardsOffset()
    {
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
    #endregion
}
