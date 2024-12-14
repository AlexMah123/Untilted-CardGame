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
    public class LoadoutCardGameplayUI : CardUI, IPointerClickHandler
    {
        [Header("LoadoutCard Gameplay Config")] 
        public FLoadoutCardObj cardInfo;

        public bool activateAfterDisplayed = true;

        public event Action<FLoadoutCardObj> OnCardClicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                //if it is activatable, activate the skill.
                if (cardInfo.upgradeSO.isActivatable)
                {
                    if (activateAfterDisplayed)
                    {
                        if (ToolTipManager.isTooltipActive)
                        {
                            OnCardClicked?.Invoke(cardInfo);
                        }
                    }
                    else
                    {
                        OnCardClicked?.Invoke(cardInfo);
                    }

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
