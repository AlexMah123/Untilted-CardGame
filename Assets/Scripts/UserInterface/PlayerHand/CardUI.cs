using System;
using System.Collections;
using System.Collections.Generic;
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
    public bool bIsSealed = true;

    //cached variables
    private Vector2 originalCardScale;
    private int originalSiblingIndex;

    //Components
    private RectTransform rectTransform;
    private Image cardImage;

    //events
    public event Action OnCardEndDrag;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        cardImage = GetComponent<Image>();

        //cache the scale and starting height of cards.
        originalCardScale = transform.localScale;
        originalSiblingIndex = transform.GetSiblingIndex();
    }

    private void Start()
    {
        cardImage.color = bIsSealed ? PlayableColor : DisabledColor;
    }

    #region Interface
    public void OnPointerEnter(PointerEventData eventData)
    {
        //move then scale up
        rectTransform.anchoredPosition3D = new Vector3(rectTransform.anchoredPosition3D.x, rectTransform.anchoredPosition3D.y + hoveredOffset.y, rectTransform.anchoredPosition3D.z);
        transform.localScale = originalCardScale * hoveredEnlargedScale;
        transform.localEulerAngles = Vector3.zero;

        transform.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //scale down then move
        gameObject.transform.localScale = originalCardScale;
        rectTransform.anchoredPosition3D = new Vector3(rectTransform.anchoredPosition3D.x, rectTransform.anchoredPosition3D.y - hoveredOffset.y, rectTransform.anchoredPosition3D.z);

        transform.SetSiblingIndex(originalSiblingIndex);

        OnCardEndDrag?.Invoke();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!bIsSealed) return;

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!bIsSealed) return;

        OnCardEndDrag?.Invoke();
    }
    #endregion
}
