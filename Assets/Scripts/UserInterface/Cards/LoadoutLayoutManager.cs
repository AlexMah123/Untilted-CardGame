using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LoadoutLayoutManager : MonoBehaviour
{
    public Transform centerPoint; // The center point of the arc
    public float radius = 5f; // Radius of the arc
    public float angleBetweenCards = 10f; // Angle between each card
    [Tooltip("The axis is rotated, so negative: up, positive: down")] public float heightOffsetFromBG = 1;

    void Start()
    {
        ArrangeCardsInArc();
    }

    void ArrangeCardsInArc()
    {
        int childCount = centerPoint.childCount;

        // Calculate the starting angle
        float totalAngle = (childCount - 1) * angleBetweenCards;
        float startAngle = -totalAngle / 2;

        for (int i = 0; i < childCount; i++)
        {
            Transform card = centerPoint.GetChild(i);
            float angle = startAngle + i * angleBetweenCards;
            float radian = angle * Mathf.Deg2Rad;

            // Calculate the card's position
            float x = centerPoint.position.x + Mathf.Cos(radian) * radius;
            float y = centerPoint.position.y + Mathf.Sin(radian) * radius;

            //height should be negate
            card.localPosition = new Vector3(x, y, card.localPosition.z + heightOffsetFromBG);

            // Optionally, rotate the card to face the center
            card.localRotation = Quaternion.Euler(0, 0, angle + 90);
            card.GetComponent<SpriteRenderer>().sortingOrder = i;
        }
    }
}