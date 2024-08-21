using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;

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


        if (SettingsGroup.activeSelf)
        {
            TimeManager.PauseTime();
        }
    }

    public void ToggleSettingsGroup(bool isOn)
    {
        if (SettingsGroup.activeSelf != isOn)
        {
            SettingsGroup.SetActive(isOn);


            if (isOn)
            {
                TimeManager.PauseTime();
            }
            else
            {
                TimeManager.ResumeTime();
            }
        }

        if (!GameplayGroup.activeSelf)
        {
            GameplayGroup.SetActive(true);
        }
    }
}
