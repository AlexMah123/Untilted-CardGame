using Game;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.GameConditionUI
{
    public class GameConditionController : MonoBehaviour
    {
        [Header("Condition Configs")]
        [SerializeField] Sprite winScreen;
        [SerializeField] Sprite loseScreen;

        [SerializeField] GameObject parentPanel;
        [SerializeField] Image conditionImage;
        [SerializeField] GameObject levelSelectButton;
        [SerializeField] GameObject rewardButton;

        private void OnDisable()
        {
            GameManager.Instance.OnLevelCompleted -= DisplayImage;
        }


        private void Start()
        {
            GameManager.Instance.OnLevelCompleted += DisplayImage;
        }

        public void DisplayImage(GameResult gameResult)
        {
            //#TODO: add win lose sound sfx

            parentPanel.SetActive(true);

            switch(gameResult)
            {
                case GameResult.Win:
                    conditionImage.sprite = winScreen;
                    levelSelectButton.SetActive(false);
                    rewardButton.SetActive(true);

                    break;

                case GameResult.Lose:
                    conditionImage.sprite = loseScreen;
                    levelSelectButton.SetActive(true);
                    rewardButton.SetActive(false);

                    break;

                default:
                    //should not reach here.
                    Debug.LogError("Win or Lost was not passed when level was complete");
                    break;
            }

            conditionImage.SetNativeSize();
        }

    
    }
}
