using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(LoadoutCardFactory))]
public class LoadoutLayoutManager : MonoBehaviour
{
    public static LoadoutLayoutManager Instance;

    [Header("Layout Config")]
    public Transform spawnContainer;
    public float radiusFromCenter = 5f; 
    public float angleBetweenCards = 10f; // temp
    [Range(1, 9)] public int displayAmount;

    [Header("Layout Offset Config")]
    [Tooltip("This value is relative to World Position")] public Vector3 offsetPosition;

    //private
    private List<UpgradeCard> cardSlots = new();

    //factory
    private LoadoutCardFactory upgradeCardFactory;

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
        InitialiseLayout();
        ArrangeCardsInArc();
    }

    private void Update()
    {
        ArrangeCardsInArc();
    }

    private void InitialiseLayout()
    {
        cardSlots.Clear();

        for(int i = 0; i < displayAmount; i++)
        {
            LoadoutCardCreationInfo creationInfo = new(spawnContainer);
            GameObject card = upgradeCardFactory.CreateUpgradeCard(creationInfo);
            UpgradeCard upgradeCard = card.GetComponent<UpgradeCard>();
            cardSlots.Add(upgradeCard);
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
            cardSlots[i].GetComponent<SpriteRenderer>().sortingOrder = i;
        }
    }

    private List<UpgradeCard> GetCardSlots()
    {
        return cardSlots;
    }

    private void UpdateCardSlots()
    {

    }
}
