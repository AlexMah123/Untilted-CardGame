using System;
using TMPro;
using UnityEngine;

namespace UserInterface.Tooltip
{
    public class ToolTipManager : MonoBehaviour
    {
        [SerializeField] private ToolTip tooltip;
        [SerializeField] RectTransform tooltipRectTransform;
        public static Action<Vector2, string, string, bool, bool> OnMouseHoverCard;
        public static Action OnMouseLoseFocusCard;
        public static bool isTooltipActive;

        private RectTransform canvasRectTransform;

        private void Awake()
        {
            tooltipRectTransform = tooltip.GetComponent<RectTransform>();
            canvasRectTransform = tooltipRectTransform.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            OnMouseHoverCard += ShowToolTip;
            OnMouseLoseFocusCard += HideTooltip;
        }

        private void OnDisable()
        {
            OnMouseHoverCard -= ShowToolTip;
            OnMouseLoseFocusCard -= HideTooltip;
        }

        private void Start()
        {
            HideTooltip();
            Cursor.visible = true;
        }
        
        private void ShowToolTip(Vector2 mousePos, string content, string header = "", bool displayInspect = true, bool displayReminder = true)
        {
            tooltip.SetText(content, header, displayInspect, displayReminder);
            
            float pivotX = mousePos.x / Screen.width;
            float pivotY = mousePos.y / Screen.height;

            //if mousePos on the leftSide/rightside of screen, then change pivot
            float finalPivotX = pivotX < 0.5f ? -0.1f : 1.01f;
            
            //if mousePos on the top/bottom of screen, then change pivot
            float finalPivotY = pivotY < 0.5f ? 0f : 1f;
            
            tooltipRectTransform.pivot = new Vector2(finalPivotX, finalPivotY);
            tooltipRectTransform.gameObject.SetActive(true);
            isTooltipActive = true;
        }

        private void HideTooltip()
        {
            tooltipRectTransform.gameObject.SetActive(false);
            isTooltipActive = false;
        }
    }
}