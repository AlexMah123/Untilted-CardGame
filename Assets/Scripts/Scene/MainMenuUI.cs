using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public Button StartButton;
    public Button SettingsButton;
    public Button ExitButton;

    private void Awake()
    {
        StartButton.onClick.AddListener(() => SceneLoader.Load(Scene.GAMESCENE));

        ExitButton.onClick.AddListener(() => {
            Application.Quit();
#if UNITY_EDITOR
            Debug.Log("Quitting Game");
#endif
        });
    }

}
