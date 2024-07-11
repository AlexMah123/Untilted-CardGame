using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [Header("Scroll Configs")]
    public float scrollSpeed = 1f;

    private bool isInScrollZone = false;

    private void Update()
    {
        if(isInScrollZone)
        {
            ProcessScroll();
        }
    }

    private void ProcessScroll()
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");

        if (scrollDelta != 0f)
        {
            Debug.Log("scrolling");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isInScrollZone = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isInScrollZone = false;
    }
}
