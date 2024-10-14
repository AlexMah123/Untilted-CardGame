using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UserInterface.Cards.Base
{
    [RequireComponent(typeof(Image))]
    public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Card Configs")]
        public Vector3 hoveredOffset;
        public float hoveredEnlargedScale = 1.5f;
        public bool shouldEnlargeOnHover = true;
        public bool shouldReorderToTop = true;

        //cached variables
        protected Vector2 originalCardScale;
        protected int originalSiblingIndex;
        protected Transform originalParent;

        //Components
        [SerializeField] protected RectTransform rectTransform;
        [SerializeField] protected Image cardImage;
        protected Canvas canvas;

        //events
        public event Action OnCardInteractEnd;

        private void Awake()
        {
            canvas = transform.root.GetComponent<Canvas>();

            //cache the scale and starting height of cards.
            originalCardScale = transform.localScale;
            originalSiblingIndex = transform.GetSiblingIndex();
        }


        #region Pointer Interface
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            //if player is already dragging a card, dont hover
            if (eventData.pointerDrag != null) return;

            SetCardHovered();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            //if player is already dragging a card, dont hover
            if (eventData.pointerDrag != null) return;

            ResetCardState();
        }
        #endregion

        protected void SetCardHovered()
        {
            //move the offset then scale up

            rectTransform.anchoredPosition3D = new Vector3(rectTransform.anchoredPosition3D.x + hoveredOffset.x, rectTransform.anchoredPosition3D.y + hoveredOffset.y, rectTransform.anchoredPosition3D.z + hoveredOffset.z);

            if (shouldEnlargeOnHover)
            {
                transform.localScale = originalCardScale * hoveredEnlargedScale;
            }

            transform.localEulerAngles = Vector3.zero;


            if (shouldReorderToTop)
            {
                transform.SetAsLastSibling();
            }
        }

        protected void ResetCardState()
        {
            //scale down then move down the offset
            if (shouldEnlargeOnHover)
            {
                gameObject.transform.localScale = originalCardScale;
            }

            rectTransform.anchoredPosition3D = new Vector3(rectTransform.anchoredPosition3D.x - hoveredOffset.x, rectTransform.anchoredPosition3D.y - hoveredOffset.y, rectTransform.anchoredPosition3D.z - hoveredOffset.z);

            if (shouldReorderToTop)
            {
                transform.SetSiblingIndex(originalSiblingIndex);
            }

            //primarily binded to PlayerHandUIManager
            OnCardInteractEnd?.Invoke();
        }
    }
}
