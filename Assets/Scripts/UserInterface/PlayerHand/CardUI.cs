using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 hoveredOffset;

    private Vector2 OriginalCardScale;
    private float startingCardHeight;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        //cache the scale and starting height of cards.
        OriginalCardScale = gameObject.transform.localScale;
        startingCardHeight = rectTransform.rect.position.y;
    }

    #region Interface
    public void OnPointerEnter(PointerEventData eventData)
    {
        //move then scale up
        rectTransform.anchoredPosition3D = new Vector3(rectTransform.anchoredPosition3D.x, rectTransform.anchoredPosition3D.y + hoveredOffset.y, rectTransform.anchoredPosition3D.z);
        gameObject.transform.localScale = OriginalCardScale * 1.5f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //scale down then move
        gameObject.transform.localScale = OriginalCardScale;
        rectTransform.anchoredPosition3D = new Vector3(rectTransform.anchoredPosition3D.x, rectTransform.anchoredPosition3D.y - hoveredOffset.y, rectTransform.anchoredPosition3D.z);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + "Being clicked on");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + "Being released on");
    }
    #endregion
}
