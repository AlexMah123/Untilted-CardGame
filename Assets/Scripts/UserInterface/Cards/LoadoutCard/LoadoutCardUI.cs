using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(InspectComponent))]
public class LoadoutCardUI : CardUI, IPointerClickHandler
{
    public LoadoutCardGOInfo cardInfo;

    public event Action<LoadoutCardGOInfo> OnCardRemovedEvent;

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
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnCardRemovedEvent?.Invoke(cardInfo);
            gameObject.SetActive(false);
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            InspectComponent.InspectCard(cardInfo.upgradeSO);
        }
    }

    public void InitializeCard(LoadoutCardGOInfo loadoutCardInfo)
    {
        cardInfo.upgradeSO = loadoutCardInfo.upgradeSO;
        cardImage.sprite = cardInfo.upgradeSO.upgradeSprite;
    }

    
}
