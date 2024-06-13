using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Layout Groups")]
    public GameObject GameplayGroup;
    public GameObject SettingsGroup;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
#if UNITY_EDITOR
        if (!GameplayGroup)
        {
            throw new System.AccessViolationException("GameplayGroup is not assigned/does not exist");
        }

        if (!SettingsGroup)
        {
            throw new System.AccessViolationException("SettingsGroup is not assigned/does not exist");
        }
#endif

    }

    public void ToggleSettingsGroup(bool isOn)
    {
        if (SettingsGroup.activeSelf != isOn)
        {
            SettingsGroup.SetActive(isOn);
        }

        if (!GameplayGroup.activeSelf)
        {
            GameplayGroup.SetActive(true);
        }
    }

    public void ExitToMainMenu()
    {
        SceneLoader.Load(Scene.MENU);
    }

}
