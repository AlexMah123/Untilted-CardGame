using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardConfirmation : MonoBehaviour, IDropHandler
{
    public static CardConfirmation Instance;

    public event Action<ChoiceCardUI> OnConfirmCardChoiceEvent;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        //if its not null and it is a CardUI type
        if (eventData.pointerDrag != null)
        {
            Debug.Log(eventData.pointerDrag.name);

            ChoiceCardUI confirmedCardUI = eventData.pointerDrag.GetComponent<ChoiceCardUI>();

            //broadcast event, primarily binded to GameManager
            OnConfirmCardChoiceEvent?.Invoke(confirmedCardUI);
        }
    }
}
