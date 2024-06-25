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
    public bool isSealed = true;

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
        cardImage.color = isSealed ? DisabledColor : PlayableColor;
    }

    public void InitialiseCard(GameChoice cardChoice, bool isChoiceAvailable)
    {
        gameChoice = cardChoice;
        isSealed = !isChoiceAvailable;
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


    public void OnBeginDrag(PointerEventData eventData)
    {
        //dont assign event data.pointerdrag if card is sealed. Automatically does not run OnDrag, OnEndDrag
        if (isSealed)
        {
            eventData.pointerDrag = null;
            return;
        }

        cardImage.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //follow the mouse cursor
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        transform.localEulerAngles = Vector3.zero;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //reset the raycast so it can be selected again.
        cardImage.raycastTarget = true;

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
