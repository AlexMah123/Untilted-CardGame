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
    public GameChoice gameChoice;
    public Vector3 hoveredOffset;
    public float hoveredEnlargedScale = 1.5f;

    //temp
    public Color PlayableColor;
    public Color DisabledColor;

    //state of card
    public bool IsSealed = true;

    //cached variables
    private Vector2 originalCardScale;
    private int originalSiblingIndex;

    //Components
    private RectTransform rectTransform;
    private Image cardImage;
    private Canvas canvas;

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

    private void Start()
    {
        cardImage.color = IsSealed ? PlayableColor : DisabledColor;
    }

    #region Interface
    public void OnPointerEnter(PointerEventData eventData)
    {
        //if player is already dragging
        if (eventData.pointerDrag != null) return;

        SetCardUIHovered();

        //#TODO: Move other cards away so its clearer?
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //if player is already dragging
        if (eventData.pointerDrag != null) return;


        ResetCardUIState();
        OnCardEndDragEvent?.Invoke();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        cardImage.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsSealed) return;

        //follow the mouse cursor
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        transform.localEulerAngles = Vector3.zero;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        cardImage.raycastTarget = true;

        if (!IsSealed) return;

        //#TODO: Move cards slower, lerp so its nicer?

        //reset the state, broadcast that you ended the drag event
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
