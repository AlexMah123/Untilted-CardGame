using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UserInterface.Cards.LoadoutCard;

namespace UserInterface.Tooltip
{
    public class CardToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float delay = 0.5f;
        [SerializeField] private bool displayInspect;
        [SerializeField] private bool displayReminder;
        
        private LoadoutCardGameplayUI cardGameplayUIAttached;
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
            cardGameplayUIAttached = GetComponent<LoadoutCardGameplayUI>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (cardGameplayUIAttached != null && cardGameplayUIAttached.cardInfo.upgradeSO != null)
            {
                //defaulted
                bool isActivatable = false; 
                
                header = cardGameplayUIAttached.cardInfo.upgradeSO.upgradeName;
                content = cardGameplayUIAttached.cardInfo.upgradeSO.upgradeDescription;

                if (displayReminder)
                {
                    isActivatable = cardGameplayUIAttached.cardInfo.upgradeSO.isActivatable;
                }
                
                tooltipTween = DOVirtual.DelayedCall(delay, () =>
                {
                    ToolTipManager.OnMouseHoverCard(Input.mousePosition, content, header, displayInspect, isActivatable);
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