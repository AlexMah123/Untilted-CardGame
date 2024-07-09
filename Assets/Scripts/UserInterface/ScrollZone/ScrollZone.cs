using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollZone : MonoBehaviour
{

    [Header("Scroll Configs")]
    public Transform cardContainer;
    public float scrollSpeed = 1f;

    private Vector3 startPointerPosition;
    private Vector3 startContainerPosition;

    private void Start()
    {
        startContainerPosition = cardContainer.position;
    }

    private void ProcessScroll()
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");

        if (scrollDelta != 0f)
        {
            Debug.Log("scrolling");
            Vector3 currentPointerPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            Vector3 offset = currentPointerPosition - startPointerPosition;

            //temp
            //cardContainer.Rotate(0f, scrollDelta * scrollSpeed, 0f, Space.World);
            //cardContainer.position = startContainerPosition + new Vector3(offset.x * scrollSpeed, 0, 0);
        }
    }

    private void OnMouseOver()
    {
        ProcessScroll();
    }

    private void OnMouseDown()
    {
        
    }

    private void OnMouseDrag()
    {
        
        
    }

    private void OnMouseUp()
    {
        
    }
}
