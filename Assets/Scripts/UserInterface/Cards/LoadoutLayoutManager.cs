using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LoadoutLayoutManager : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject cardObjPrefab;

    [Header("Layout Config")]
    public Transform centerPoint;
    public float radiusFromCenter = 5f; 
    public float angleBetweenCards = 10f; // temp
    [Range(1, 9)] public int displayAmount;

    [Header("Layout Offset Config")]
    [Tooltip("This value is relative to World Position")] public Vector3 offsetPosition;

    [HideInInspector]
    public List<GameObject> cardSlots = new();

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
            GameObject card = Instantiate(cardObjPrefab, centerPoint);
            cardSlots.Add(card);
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
            float x = centerPoint.position.x + Mathf.Cos(radian) * radiusFromCenter;
            float y = centerPoint.position.y + Mathf.Sin(radian) * radiusFromCenter;

            //adjust position
            cardSlots[i].gameObject.transform.localPosition = new Vector3(x, y, 0);
            cardSlots[i].gameObject.transform.position += new Vector3(offsetPosition.x, offsetPosition.y, offsetPosition.z);

            //rotate cards outwards
            cardSlots[i].gameObject.transform.localRotation = Quaternion.Euler(0, 0, angle + 90);
            cardSlots[i].GetComponent<SpriteRenderer>().sortingOrder = i;
        }
    }
}
