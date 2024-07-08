using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChoiceCardUI : CardUI
{
    public GameChoice gameChoice;

    //temp
    public Color PlayableColor;
    public Color DisabledColor;

    //state of card
    public bool isSealed = true;
    public bool isMoveable = true;

    public bool IsSealed
    {
        get { return isSealed; }
        set
        {
            isSealed = value;
            cardImage.color = IsSealed ? DisabledColor : PlayableColor;
        }
    }

    public void InitialiseCard(GameChoice cardChoice, bool isChoiceAvailable, bool isInteractable)
    {
        gameChoice = cardChoice;
        IsSealed = !isChoiceAvailable;
        isMoveable = isInteractable;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        //dont assign event data.pointerdrag if card is sealed. Automatically does not run OnDrag, OnEndDrag
        if (IsSealed || !isMoveable)
        {
            eventData.pointerDrag = null;
            return;
        }

        base.OnBeginDrag(eventData);
    }
}
