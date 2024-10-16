using PlayerCore.PlayerComponents;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UserInterface.Cards.Base;

namespace UserInterface.Cards.ChoiceCard
{
    public class ChoiceCardUI : CardUI, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Choice Card Configs")]
        public Image sealedEffectImage;
        public GameChoice gameChoice;

        public Color PlayableColor;
        public Color DisabledColor;

        //state of card
        public bool isSealed = true;
        public bool isMoveable = true;

        public bool IsSealed
        {
            get => isSealed;
            set
            {
                isSealed = value;
                cardImage.color = IsSealed ? DisabledColor : PlayableColor;

                //only change the state if its not the same as the bool
                if (sealedEffectImage.gameObject.activeSelf != isSealed)
                    sealedEffectImage.gameObject.SetActive(isSealed);
            }
        }

        public void InitialiseCard(GameChoice cardChoice, bool isChoiceAvailable, bool isInteractable)
        {
            gameChoice = cardChoice;
            IsSealed = !isChoiceAvailable;
            isMoveable = isInteractable;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //only proceed if left mouse button is used.
            //dont assign event data.pointerdrag if card is sealed. Automatically does not run OnDrag, OnEndDrag
            if (eventData.button != PointerEventData.InputButton.Left || IsSealed || !isMoveable)
            {
                eventData.pointerDrag = null;
                return;
            }

            cardImage.raycastTarget = false;

            //caching the parent (hand display) and setting it to the canvas
            originalParent = transform.parent;
            transform.SetParent(transform.root);
        }

        public void OnDrag(PointerEventData eventData)
        {
            //only proceed if left mouse button is used.
            if (eventData.button != PointerEventData.InputButton.Left) return;

            //follow the mouse cursor
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            transform.localEulerAngles = Vector3.zero;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //only proceed if left mouse button is used.
            if (eventData.button != PointerEventData.InputButton.Left) return;

            //reset the raycast so it can be selected again.
            cardImage.raycastTarget = true;

            //setting the parent back to the hand display
            transform.SetParent(originalParent);

            //#TODO: Move cards slower, lerp so its nicer?

            //reset the state, broadcast that you ended the drag event, primarily binded to PlayerHandUIManager
            ResetCardState();
        }
    }
}
