using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public struct LoadoutCardGOInfo
{
    public LoadoutCardGOInfo(UpgradeDefinitionSO _upgradeSO)
    {
        upgradeSO = _upgradeSO;
    }

    public UpgradeDefinitionSO upgradeSO;
}

public class LoadoutCardGO : MonoBehaviour
{
    [Header("Card Visual Configs")]
    public SpriteRenderer cardSpriteRenderer;
    public SpriteRenderer lockedSpriteRenderer;
    public Vector3 hoveredOffset;
    public float hoveredEnlargedScale = 1.5f;

    //state of card
    public Color UnlockedColor;
    public Color LockedColor;
    public bool isLocked = false;

    [Header("Card Configs")]
    public LoadoutCardGOInfo cardInfo;

    //private
    private Vector2 originalCardScale;
    private int originalSortingOrder;

    private bool isHoveredOver;

    //events
    public event Action OnCardEndInteractEvent;
    public event Action<LoadoutCardGOInfo> OnCardSelectedEvent;

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
            cardInfo.upgradeSO = upgradeSO;
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
        ResetCardState();
    }

    private void OnMouseDown()
    {
        
    }

    private void OnMouseUpAsButton()
    {
        if (IsLocked)
        {
            return;
        }

        //binded primarily to LoadoutLayoutManager
        OnCardSelectedEvent?.Invoke(cardInfo);
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

        //bind if needed to. 
        OnCardEndInteractEvent?.Invoke();
    }
}
