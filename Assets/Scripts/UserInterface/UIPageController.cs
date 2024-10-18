using System;
using UnityEngine;
using UnityEngine.UI;
using UserInterface.LoadoutSelection;

namespace UserInterface
{
    public enum SnapType
    {
        Top,
        Middle,
        Bottom,
        Left,
        Right,
    }

    public enum PageType
    {
        Horizontal,
        Vertical,
    }
    
    public class UIPageController : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private RectTransform contentParent;
        [SerializeField] private RectTransform contentPanel;
        [SerializeField] Button previousButton;
        [SerializeField] Button nextButton;

        [Header("Configs")] 
        [SerializeField] private Vector2 contentOffset = Vector2.zero;
        [SerializeField] private PageType pageType = PageType.Horizontal; 
        [SerializeField] private SnapType snapType = SnapType.Top;
        
        private int currentPageIndex = 0;
        private int totalContentCount;
        
        public event Action OnPageUpdated;

        private void Awake()
        {
            totalContentCount = contentPanel.childCount;
        }

        private void Start()
        {
            InitializePage();
            Invoke(nameof(UpdateButtonState), 0.1f);
        }

        private void InitializePage()
        {
            if (totalContentCount == 0)
            {
                Debug.LogError("There is no content to display.");
                return;
            }

            SnapTo(contentPanel.GetChild(0) as RectTransform);
        }

        public void PreviousPage()
        {
            ChangePage(-1);
        }

        public void NextPage()
        {
            ChangePage(1);
        }

        private void ChangePage(int direction)
        {
            currentPageIndex += direction;
            
            currentPageIndex = Mathf.Clamp(currentPageIndex, 0, totalContentCount - 1);

            // Snap to the target page
            SnapTo(contentPanel.GetChild(currentPageIndex) as RectTransform);
            
            UpdateButtonState();

            OnPageUpdated?.Invoke();
        }

        protected virtual void UpdateButtonState()
        {
            previousButton.gameObject.SetActive(currentPageIndex > 0);
            nextButton.gameObject.SetActive(currentPageIndex < totalContentCount - 1);
        }

        public void SnapTo(RectTransform target)
        {
            //#TODO: add some sort of animation like lerping
            
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel);
            
            // Get the position of the target relative to contentParent
            Vector2 targetPosition = contentParent.transform.InverseTransformPoint(target.position);
            Vector2 contentPanelPosition = contentParent.transform.InverseTransformPoint(contentPanel.position);

            Vector2 deltaPosition = contentPanelPosition - targetPosition;

            // Horizontal snapping
            if (pageType == PageType.Horizontal)
            {
                switch (snapType)
                {
                    case SnapType.Left:
                        deltaPosition.x += 0;
                        break;
                    
                    case SnapType.Middle:
                        deltaPosition.x += (contentParent.rect.width / 2) - (target.rect.width / 2);
                        break;

                    case SnapType.Right:
                        deltaPosition.x += contentParent.rect.width - target.rect.width;
                        break;
                    
                    //default to left
                    default:
                        deltaPosition.x += 0;
                        break;
                }
            }
            else if(pageType == PageType.Vertical)
            {
                switch (snapType)
                {
                    case SnapType.Top:
                        deltaPosition.y += 0;
                        break;
                    case SnapType.Middle:
                        // Snap to the middle of the contentParent
                        deltaPosition.y += (contentParent.rect.height / 2) - (target.rect.height / 2);
                        break;
                    
                    case SnapType.Bottom:
                        deltaPosition.y += contentParent.rect.height - target.rect.height;
                        break;
                    
                    //default to top
                    default:
                        deltaPosition.y += 0;
                        break;
                }
            }

            deltaPosition += contentOffset;
            
            // Apply the new anchored position
            contentPanel.anchoredPosition = deltaPosition;
        }
    }
}
