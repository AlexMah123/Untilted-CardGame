using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using LoadoutSelection;
using UserInterface.Cards.Base;
using UserInterface.InspectUpgradeUI;

namespace UserInterface.Cards.LoadoutCard
{
    [RequireComponent(typeof(InspectComponent), typeof(Outline))]
    public class LoadoutCardUI : CardUI, IPointerClickHandler
    {
        [Header("Loadout Card Config")]
        public FLoadoutCardObj cardInfo;
        public bool shouldDisableOnClick = true;
        public bool shouldDisplayOutline = true;

        public event Action<FLoadoutCardObj> OnCardClicked;

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            if(shouldDisplayOutline)
            {
                gameObject.GetComponent<Outline>().enabled = true;
            }

        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            if (shouldDisplayOutline)
            {
                gameObject.GetComponent<Outline>().enabled = false;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnCardClicked?.Invoke(cardInfo);

                if(shouldDisableOnClick)
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
