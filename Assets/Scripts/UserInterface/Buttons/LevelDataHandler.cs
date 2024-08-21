using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelDataHandler : MonoBehaviour
{
    [Header("UI Component")]
    public Image thumbnailImage;
    public TextMeshProUGUI levelNameText;

    [Header("Level Data")]
    public LevelConfigSO levelConfig;

    private void Start()
    {
        if (levelConfig != null)
        {
            thumbnailImage.sprite = levelConfig.levelImage;
            levelNameText.text = levelConfig.levelName;
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
