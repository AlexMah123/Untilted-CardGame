using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveButtonHandler : MonoBehaviour
{
    [SerializeField] private Button saveButton;

    private void Start()
    {
        if (saveButton != null)
        {
            saveButton.onClick.AddListener(OnSaveButtonClicked);
        }
    }

    private void OnSaveButtonClicked()
    {
        if (SaveSystemManager.Instance != null)
        {
            SaveSystemManager.Instance.SaveGame();
        }
        else
        {
            Debug.LogError("SaveSystemManager instance is missing");
        }
    }
}
