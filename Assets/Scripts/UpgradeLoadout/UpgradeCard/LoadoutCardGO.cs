using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoadoutCardGO : MonoBehaviour
{
    [Header("Card Configs")]
    public Vector3 hoveredOffset;
    public float hoveredEnlargedScale = 1.5f;

    //private
    private Vector2 originalCardScale;
    private SpriteRenderer spriteRenderer;
    private int originalSortingOrder;

    //events
    public event Action OnCardEndInteractEvent;
    public event Action OnCardSelectedEvent;

    private void Awake()
    {
        originalCardScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSortingOrder = spriteRenderer.sortingOrder;
    }

    private void OnMouseEnter()
    {
        SetCardHovered();
    }

    private void OnMouseExit()
    {
        //reset the card layout and then broadcast event
        ResetCardState();
    }

    private void OnMouseDown()
    {
        //reset the card layout and then broadcast event
        ResetCardState();
        OnCardSelectedEvent?.Invoke();
    }

    protected void SetCardHovered()
    {
        //move the offset then scale up
        transform.localPosition = new Vector3(transform.localPosition.x + hoveredOffset.x, transform.localPosition.y + hoveredOffset.y, transform.localPosition.z + hoveredOffset.z);
        transform.localScale = originalCardScale * hoveredEnlargedScale;
        spriteRenderer.sortingOrder = 99;
    }

    protected void ResetCardState()
    {
        //scale down then move down the offset
        gameObject.transform.localScale = originalCardScale;
        transform.localPosition = new Vector3(transform.localPosition.x - hoveredOffset.x, transform.localPosition.y - hoveredOffset.y, transform.localPosition.z - hoveredOffset.z);
        spriteRenderer.sortingOrder = originalSortingOrder;

        //broacast event, primarily binded to LoadoutLayoutManager
        OnCardEndInteractEvent?.Invoke();
    }
}
