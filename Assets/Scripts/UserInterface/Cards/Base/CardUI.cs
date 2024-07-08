using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Card Configs")]
    public Vector3 hoveredOffset;
    public float hoveredEnlargedScale = 1.5f;

    //cached variables
    protected Vector2 originalCardScale;
    protected int originalSiblingIndex;
    protected Transform originalParent;

    //Components
    protected RectTransform rectTransform;
    protected Image cardImage;
    protected Canvas canvas;

    //events
    public event Action OnCardEndDragEvent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        cardImage = GetComponent<Image>();
        canvas = transform.root.GetComponent<Canvas>();

        //cache the scale and starting height of cards.
        originalCardScale = transform.localScale;
        originalSiblingIndex = transform.GetSiblingIndex();
    }


    #region Pointer Interface
    public void OnPointerEnter(PointerEventData eventData)
    {
        //if player is already dragging a card, dont hover
        if (eventData.pointerDrag != null) return;

        SetCardUIHovered();
        //#TODO: Move other cards away so its clearer?
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //if player is already dragging a card, dont hover
        if (eventData.pointerDrag != null) return;

        ResetCardUIState();
        OnCardEndDragEvent?.Invoke();
    }


    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        cardImage.raycastTarget = false;

        //caching the parent (hand display) and setting it to the canvas
        originalParent = transform.parent;
        transform.SetParent(transform.root);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        //follow the mouse cursor
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        transform.localEulerAngles = Vector3.zero;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        //reset the raycast so it can be selected again.
        cardImage.raycastTarget = true;

        //setting the parent back to the hand display
        transform.SetParent(originalParent);

        //#TODO: Move cards slower, lerp so its nicer?

        //reset the state, broadcast that you ended the drag event, primarily binded to PlayerHandUIManager
        ResetCardUIState();
        OnCardEndDragEvent?.Invoke();
    }
    #endregion

    private void SetCardUIHovered()
    {
        //move the offset then scale up

        rectTransform.anchoredPosition3D = new Vector3(rectTransform.anchoredPosition3D.x, rectTransform.anchoredPosition3D.y + hoveredOffset.y, rectTransform.anchoredPosition3D.z);
        transform.localScale = originalCardScale * hoveredEnlargedScale;
        transform.localEulerAngles = Vector3.zero;

        transform.SetAsLastSibling();
    }

    private void ResetCardUIState()
    {
        //scale down then move down the offset
        gameObject.transform.localScale = originalCardScale;
        rectTransform.anchoredPosition3D = new Vector3(rectTransform.anchoredPosition3D.x, rectTransform.anchoredPosition3D.y - hoveredOffset.y, rectTransform.anchoredPosition3D.z);

        transform.SetSiblingIndex(originalSiblingIndex);
    }
}
