using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCondition : MonoBehaviour
{
    [Header("Condition Configs")]
    [SerializeField] Sprite winScreen;
    [SerializeField] Sprite loseScreen;

    [SerializeField] GameObject parentPanel;
    [SerializeField] Image conditionImage;
    [SerializeField] GameObject levelSelectButton;

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
        parentPanel.SetActive(true);

        switch(gameResult)
        {
            case GameResult.Win:
                conditionImage.sprite = winScreen;
                levelSelectButton.SetActive(false);
                break;

            case GameResult.Lose:
                conditionImage.sprite = loseScreen;
                levelSelectButton.SetActive(true);
                break;

            default:
                //should not reach here.
                Debug.LogError("Win or Lost was not passed when level was complete");
                break;
        }

        conditionImage.SetNativeSize();
    }

    
}
