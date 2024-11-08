using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UserInterface.Cards.LoadoutCard;

namespace UserInterface.Tooltip
{
    public class CardToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private LoadoutCardUI cardUIAttached;
        [SerializeField] private float delay = 0.5f;
        [SerializeField] private bool displayInspect;
        
        private bool displayReminder;
        private string header = string.Empty;
        private string content = string.Empty;

        private Tween tooltipTween;
        
        private void OnDisable()
        {
            if(ToolTipManager.OnMouseLoseFocusCard != null)
            {
                ToolTipManager.OnMouseLoseFocusCard();
            }
        }

        private void Awake()
        {
            cardUIAttached = GetComponent<LoadoutCardUI>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (cardUIAttached != null && cardUIAttached.cardInfo.upgradeSO != null)
            {
                header = cardUIAttached.cardInfo.upgradeSO.upgradeName;
                content = cardUIAttached.cardInfo.upgradeSO.upgradeDescription;
                displayReminder = cardUIAttached.cardInfo.upgradeSO.isActivatable;
                
                tooltipTween = DOVirtual.DelayedCall(delay, () =>
                {
                    ToolTipManager.OnMouseHoverCard(Input.mousePosition, content, header, displayInspect, displayReminder);
                });
            }
            else
            {
                Debug.LogError("CardToolTipTrigger is missing cardUIAttached");
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (tooltipTween != null && tooltipTween.IsActive())
            {
                DOTween.KillAll(false, false);
            }
            
            ToolTipManager.OnMouseLoseFocusCard();
        }
    }
}