using TMPro;
using UnityEngine;
using UnityEngine.UI;

using LevelConfig.Base;
using LevelManager;

namespace UserInterface.Buttons
{
    public class LevelDataHandler : MonoBehaviour
    {
        [Header("UI Component")]
        public Image thumbnailImage;
        public Image completedOverlay;
        public TextMeshProUGUI levelNameText;
        
        [Header("Locked Config")]
        public Color lockedColor;

        [Header("Level Data")]
        public LevelConfigSO levelConfig;

        private void Start()
        {
            if (levelConfig != null)
            {
                thumbnailImage.sprite = levelConfig.levelImage;
                levelNameText.text = levelConfig.levelName;

                bool isCompleted = LevelDataManager.Instance.IsLevelCompleted(levelConfig.levelName);
                thumbnailImage.color = isCompleted ? lockedColor : Color.white;
                completedOverlay.gameObject.SetActive(isCompleted);
            }
            else
            {
                throw new MissingReferenceException($"Level Config Data is missing from {gameObject}");
            }

        }

        public void SelectLevel()
        {
            LevelDataManager.Instance.currentSelectedLevelSO = levelConfig;
        }
    }
}
