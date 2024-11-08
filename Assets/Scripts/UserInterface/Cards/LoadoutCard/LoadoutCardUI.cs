using System;
using GameCore.LoadoutSelection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UserInterface.Cards.Base;
using UserInterface.InspectUpgradeUI;

namespace UserInterface.Cards.LoadoutCard
{
    [RequireComponent(typeof(InspectComponent))]
    public class LoadoutCardUI : CardUI, IPointerClickHandler
    {
        [Header("Loadout Card Config")] 
        public FLoadoutCardObj cardInfo;
        public bool shouldDisableOnClick = true;
        
        public Outline outlineComponent;
        public bool shouldDisplayOutline = true;

        public event Action<FLoadoutCardObj> OnCardClicked;


        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            if (shouldDisplayOutline)
            {
                outlineComponent.enabled = true;
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            if (shouldDisplayOutline)
            {
                outlineComponent.enabled = false;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (cardInfo.upgradeSO.isActivatable)
                {
                    OnCardClicked?.Invoke(cardInfo);
                }

                if (shouldDisableOnClick)
                {
                    gameObject.SetActive(false);
                }
            }

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                InspectComponent.InspectCard(cardInfo.upgradeSO);
            }
        }

        public void InitializeCard(FLoadoutCardObj fLoadoutCardInfo)
        {
            cardInfo.upgradeSO = fLoadoutCardInfo.upgradeSO;
            cardImage.sprite = cardInfo.upgradeSO.upgradeSprite;
        }
    }
}