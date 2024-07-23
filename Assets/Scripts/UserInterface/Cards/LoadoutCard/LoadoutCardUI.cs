using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LoadoutCardUI : CardUI
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

    public void InitializeCard(UpgradeDefinitionSO upgradeSO)
    {
        cardImage.sprite = upgradeSO.upgradeSprite;
    }

}
