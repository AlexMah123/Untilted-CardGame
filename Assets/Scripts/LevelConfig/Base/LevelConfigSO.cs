using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/LevelConfigSO")]
public class LevelConfigSO : ScriptableObject
{
    [Header("Human Player")]
    [SerializeField] PlayerData humanPlayer;

    [Header("Ai Player")]
    [SerializeField] PlayerData aiPlayer;

    [Header("UI Data")]
    public string levelName;
    public Sprite levelImage;
}
