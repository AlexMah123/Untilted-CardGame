using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(InspectComponent))]
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
    private Vector3 originalCardScale;
    private int originalSortingOrder;

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

    private void Update()
    {
        HandleOnInspected();
        HandleOnSelected();
    }

    private void HandleOnSelected()
    {
        if (Input.GetMouseButtonUp((int)MouseButton.Left))
        {
            if (EventSystem.current.IsPointerOverGameObject() || IsLocked) return;

            if (IsPointerOverGameObject())
            {
                ResetCardState();

                //binded primarily to LoadoutLayoutManager
                OnCardSelectedEvent?.Invoke(cardInfo);
            }
        }
    }

    private void HandleOnInspected()
    {
        if (Input.GetMouseButtonUp((int)MouseButton.Right))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            if (IsPointerOverGameObject())
            {
                ResetCardState();

                //binded primarily to InspectUI
                InspectComponent.InspectCard(cardInfo.upgradeSO);
            }
        }
    }

    public void InitialiseLoadoutGO(LoadoutCardGOInfo loadoutCardInfo, bool isLocked)
    {
        if(loadoutCardInfo.upgradeSO != null)
        {
            cardInfo.upgradeSO = loadoutCardInfo.upgradeSO;
            cardSpriteRenderer.sprite = cardInfo.upgradeSO.upgradeSprite;
        }
        else
        {
            cardSpriteRenderer.sprite = null;
        }

        IsLocked = isLocked;
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        SetCardHovered();
    }

    private void OnMouseExit()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        ResetCardState();
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

    protected bool IsPointerOverGameObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.transform == transform;
        }
        return false;
    }
}
