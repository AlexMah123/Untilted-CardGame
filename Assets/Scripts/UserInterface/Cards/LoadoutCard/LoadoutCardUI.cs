using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LoadoutCardUI : CardUI, IPointerClickHandler
{
    public event Action OnCardInspectEvent;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        OnCardInspectEvent?.Invoke();
        Debug.Log("Show Details, have a remove button at btm");

    }

    public void InitializeCard(UpgradeDefinitionSO upgradeSO)
    {
        cardImage.sprite = upgradeSO.upgradeSprite;
    }

    
}
