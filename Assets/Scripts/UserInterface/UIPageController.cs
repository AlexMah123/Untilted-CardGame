using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using LevelCore.LevelManager;
using UserInterface.Buttons;

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
        [SerializeField] float animationTime = 0.25f;
        [SerializeField] private float selectedScale = 1.25f;
        [SerializeField] private float defaultScale = 0.75f;

        private List<RectTransform> contentList = new();
        private RectTransform lastSelectedLevel = null;
        private int currentContentIndex = 0;
        private int totalContentCount;

        public event Action OnPageUpdated;

        private void Awake()
        {
            InitializeController();
        }

        private void Start()
        {
            InitializePage();
        }

        private void InitializePage()
        {
            if (totalContentCount == 0)
            {
                Debug.LogError("There is no content to display.");
                return;
            }

            var currentSelectedLevel = LevelDataManager.Instance.currentSelectedLevelSO;
            if (currentSelectedLevel != null)
            {
                LevelDataHandler currentLevel = contentPanel.Cast<Transform>()
                    .Select(x => x.GetComponent<LevelDataHandler>())
                    .FirstOrDefault(handler => handler != null && handler.levelConfig == currentSelectedLevel);
                
                if (currentLevel != null)
                {
                    currentContentIndex = contentList.IndexOf(currentLevel.transform as RectTransform);
                }
            }
            else
            {
                currentContentIndex = 0;
            }

            SnapTo(contentPanel.GetChild(currentContentIndex) as RectTransform);
            
            Invoke(nameof(UpdateButtonState), 0.1f);
            Invoke(nameof(UpdateCurrentScale), 0.1f);
        }


        private void InitializeController()
        {
            totalContentCount = contentPanel.childCount;
            foreach (Transform child in contentPanel)
            {
                var currentContent = child.GetComponent<RectTransform>();
                if (currentContent != null)
                {
                    contentList.Add(currentContent);
                }

                var button = child.GetComponent<Button>();
                var sceneTransitionHandler = child.GetComponent<SceneTransitionHandler>();
                var levelDataHandler = child.GetComponent<LevelDataHandler>();
                if (button && sceneTransitionHandler && levelDataHandler)
                {
                    button.onClick.AddListener(() =>
                        HandleLevelOnClick(currentContent, sceneTransitionHandler, levelDataHandler));
                }
            }
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
            currentContentIndex += direction;

            currentContentIndex = Mathf.Clamp(currentContentIndex, 0, totalContentCount - 1);

            // Snap to the target page
            SnapTo(contentPanel.GetChild(currentContentIndex) as RectTransform);
            OnPageUpdated?.Invoke();
        }

        private void HandleLevelOnClick(RectTransform clickedLevel, SceneTransitionHandler sceneTransitionHandler,
            LevelDataHandler levelDataHandler)
        {
            if (lastSelectedLevel == clickedLevel)
            {
                levelDataHandler.SelectLevel();
                sceneTransitionHandler.LoadScene();
            }
            else
            {
                int pageDelta = contentList.IndexOf(clickedLevel) - currentContentIndex;

                ChangePage(pageDelta);
            }
        }

        private void UpdateButtonState()
        {
            previousButton.gameObject.SetActive(currentContentIndex > 0);
            nextButton.gameObject.SetActive(currentContentIndex < totalContentCount - 1);
        }

        private void UpdateCurrentScale()
        {
            contentList[currentContentIndex].DOScale(new Vector3(selectedScale, selectedScale, 1f), animationTime)
                .SetEase(Ease.OutSine);

            for (int i = 0; i < contentList.Count; i++)
            {
                if (i == currentContentIndex) continue;

                contentList[i].DOScale(defaultScale, animationTime).SetEase(Ease.OutSine);
            }
        }

        public void SnapTo(RectTransform target)
        {
            lastSelectedLevel = target;

            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel);

            // Get the position of the target relative to contentParent
            Vector2 targetPosition = contentParent.transform.InverseTransformPoint(target.position);
            Vector2 contentPanelPosition = contentParent.transform.InverseTransformPoint(contentPanel.position);

            Vector2 deltaPosition = contentPanelPosition - targetPosition;

            // Horizontal snapping (pivot is 0.5, 0.5)
            if (pageType == PageType.Horizontal)
            {
                switch (snapType)
                {
                    case SnapType.Left:
                        deltaPosition.x += target.rect.width / 2;
                        break;

                    case SnapType.Middle:
                        deltaPosition.x += (contentParent.rect.width / 2);
                        break;

                    case SnapType.Right: // needs to be inframe so offset minus
                        deltaPosition.x += contentParent.rect.width - (target.rect.width / 2);
                        break;

                    //default to left
                    default:
                        deltaPosition.x += target.rect.width / 2;
                        break;
                }
            }
            else if (pageType == PageType.Vertical)
            {
                switch (snapType)
                {
                    case SnapType.Top:
                        deltaPosition.y += target.rect.height / 2;
                        break;

                    case SnapType.Middle:
                        deltaPosition.y += (contentParent.rect.height / 2);
                        break;

                    case SnapType.Bottom:
                        deltaPosition.y += contentParent.rect.height - (target.rect.height / 2);
                        break;

                    //default to top
                    default:
                        deltaPosition.y += target.rect.height / 2;
                        break;
                }
            }

            deltaPosition += contentOffset;

            if (pageType == PageType.Horizontal)
            {
                contentPanel.DOAnchorPosX(deltaPosition.x, animationTime);
            }
            else if (pageType == PageType.Vertical)
            {
                contentPanel.DOAnchorPosY(deltaPosition.y, animationTime);
            }

            UpdateButtonState();
            UpdateCurrentScale();
        }
    }
}