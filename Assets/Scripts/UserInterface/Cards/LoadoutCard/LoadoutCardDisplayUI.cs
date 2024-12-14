using System;
using GameCore.LoadoutSelection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UserInterface.Cards.Base;
using UserInterface.InspectUpgradeUI;
using UserInterface.Tooltip;

namespace UserInterface.Cards.LoadoutCard
{
    [RequireComponent(typeof(InspectComponent))]
    public class LoadoutCardDisplayUI : CardUI, IPointerClickHandler
    {
        [Header("LoadoutCard Display Config")] 
        public bool shouldDisableOnClick = true;
        public FLoadoutCardObj cardInfo;

        public event Action<FLoadoutCardObj> OnCardClicked;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnCardClicked?.Invoke(cardInfo);
                
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