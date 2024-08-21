using UnityEngine;

[CreateAssetMenu(menuName = "Level/LevelConfigSO")]
public class LevelConfigSO : ScriptableObject
{
    [Header("Human Player")]
    public PlayerData humanPlayer;

    [Header("Ai Player")]
    public PlayerData aiPlayer;

    [Header("UI Data")]
    public string levelName;
    public Sprite levelImage;
}
