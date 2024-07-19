using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoadoutCardGO : MonoBehaviour
{
    [Header("Card Configs")]
    public SpriteRenderer cardSpriteRenderer;
    public SpriteRenderer lockedSpriteRenderer;
    public Vector3 hoveredOffset;
    public float hoveredEnlargedScale = 1.5f;

    //state of card
    public Color UnlockedColor;
    public Color LockedColor;
    public bool isLocked = false;

    //private
    private Vector2 originalCardScale;
    private int originalSortingOrder;

    //events
    public event Action OnCardEndInteractEvent;
    public event Action OnCardSelectedEvent;

    public bool IsLocked
    {
        get => isLocked;
        set
        {
            isLocked = value;
            cardSpriteRenderer.color = IsLocked ? LockedColor : UnlockedColor;

            //only change the state if its not the same as the bool
            if (lockedSpriteRenderer.gameObject.activeSelf != isLocked)
                lockedSpriteRenderer.gameObject.SetActive(isLocked);
        }
    }

    private void Awake()
    {
        originalCardScale = transform.localScale;
    }

    public void InitialiseLoadoutGO(UpgradeDefinitionSO upgradeSO, bool isLocked)
    {
        if(upgradeSO != null)
        {
            cardSpriteRenderer.sprite = upgradeSO.upgradeSprite;
        }
        else
        {
            cardSpriteRenderer.sprite = null;
        }

        IsLocked = isLocked;
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
        //ResetCardState();
        OnCardSelectedEvent?.Invoke();
    }

    protected void SetCardHovered()
    {
        //move the offset then scale up
        transform.localPosition = new Vector3(transform.localPosition.x + hoveredOffset.x, transform.localPosition.y + hoveredOffset.y, transform.localPosition.z + hoveredOffset.z);
        transform.localScale = originalCardScale * hoveredEnlargedScale;
        cardSpriteRenderer.sortingOrder = 99;
        lockedSpriteRenderer.sortingOrder = 100;
    }

    protected void ResetCardState()
    {
        //scale down then move down the offset
        gameObject.transform.localScale = originalCardScale;
        transform.localPosition = new Vector3(transform.localPosition.x - hoveredOffset.x, transform.localPosition.y - hoveredOffset.y, transform.localPosition.z - hoveredOffset.z);
        cardSpriteRenderer.sortingOrder = originalSortingOrder;
        lockedSpriteRenderer.sortingOrder = originalSortingOrder + 1;

        //bind if needed
        OnCardEndInteractEvent?.Invoke();
    }
}
