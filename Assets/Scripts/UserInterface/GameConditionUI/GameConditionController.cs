using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Game;
using LevelManager;

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
        [SerializeField] private TextMeshProUGUI conditionText;

        private void OnDisable()
        {
            GameManager.Instance.OnLevelCompleted -= DisplayImage;
        }


        private void Start()
        {
            GameManager.Instance.OnLevelCompleted += DisplayImage;
            
            //defaulted to be empty and hidden
            conditionText.gameObject.SetActive(false);
            conditionText.text = String.Empty;
        }

        private void DisplayImage(GameResult gameResult)
        {
            //#TODO: add win lose sound sfx

            parentPanel.SetActive(true);

            switch(gameResult)
            {
                case GameResult.Win:
                    conditionImage.sprite = winScreen;
                    
                    //if level name in totalLevels is equal and isCompleted is false, enable claim reward button.
                    if(LevelDataManager.Instance.IsLevelCompleted(LevelDataManager.Instance.currentSelectedLevelSO.levelName) == false)
                    {
                        EnableClaimReward(true);
                        break;
                    }

                    //if reached here, means level is completed.
                    conditionText.gameObject.SetActive(true);
                    conditionText.text = "Level Completed already!";
                    EnableClaimReward(false);
                    break;

                case GameResult.Lose:
                    conditionImage.sprite = loseScreen;
                    EnableClaimReward(false);
                    break;

                default:
                    //should not reach here.
                    Debug.LogError("Win or Lost was not passed when level was complete");
                    break;
            }

            conditionImage.SetNativeSize();
        }

        private void EnableClaimReward(bool state)
        {
            //if state is true, enable claim reward button, hide level select button.
            //vice versa.
            levelSelectButton.SetActive(!state);
            rewardButton.SetActive(state);
        }
        
    }
}
