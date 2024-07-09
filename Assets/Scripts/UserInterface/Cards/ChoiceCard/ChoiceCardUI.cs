using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoiceCardUI : CardUI
{
    [Header("Choice Card Configs")]
    public Image sealedEffectImage;
    public GameChoice gameChoice;

    public Color PlayableColor;
    public Color DisabledColor;

    //state of card
    public bool isSealed = true;
    public bool isMoveable = true;

    public bool IsSealed
    {
        get => isSealed;
        set
        {
            isSealed = value;
            cardImage.color = IsSealed ? DisabledColor : PlayableColor;

            //only change the state if its not the same as the bool
            if (sealedEffectImage.gameObject.activeSelf != isSealed)
                sealedEffectImage.gameObject.SetActive(isSealed);
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
