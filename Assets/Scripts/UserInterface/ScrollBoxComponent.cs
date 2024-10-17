using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public enum SnapType
    {
        Top,
        Middle,
        Bottom
    }
    
    public class ScrollBoxComponent : MonoBehaviour
    {
        private ScrollRect scrollRect;
        private RectTransform contentPanel;
        
        

        public void Awake()
        {
            scrollRect = GetComponent<ScrollRect>();
            contentPanel = scrollRect.content;
        }

        public void SnapTo(RectTransform target)
        {
            Canvas.ForceUpdateCanvases();

            contentPanel.anchoredPosition =
                (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position)
                - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
        }
    }
}
