using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UserInterface.Cards.ChoiceCard;

namespace UserInterface.ConfirmationZone
{
    public class CardConfirmation : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public static CardConfirmation Instance;

        public event Action<ChoiceCardUI> OnConfirmCardChoice;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            //if its not null and it is a CardUI type
            if (eventData.pointerDrag != null)
            {
                ChoiceCardUI confirmedCardUI = eventData.pointerDrag.GetComponent<ChoiceCardUI>();

                //broadcast event, primarily binded to GameManager
                OnConfirmCardChoice?.Invoke(confirmedCardUI);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                ChoiceCardUI confirmedCardUI = eventData.pointerDrag.GetComponent<ChoiceCardUI>();

                if(confirmedCardUI)
                {
                    confirmedCardUI.GetComponent<Outline>().enabled = true;
                }

            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                ChoiceCardUI confirmedCardUI = eventData.pointerDrag.GetComponent<ChoiceCardUI>();

                if (confirmedCardUI)
                {
                    confirmedCardUI.GetComponent<Outline>().enabled = false;
                }

            }
        }
    }
}
