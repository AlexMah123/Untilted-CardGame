using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InspectUpgradeUI : MonoBehaviour
{
    [Header("Parent")]
    [SerializeField] GameObject inspectUI;

    [Header("Content")]
    [SerializeField] Image upgradeImage;
    [SerializeField] TextMeshProUGUI upgradeName;
    [SerializeField] TextMeshProUGUI upgradeDescription;

    //flag
    private bool isOnCardInspectEventBinded = false;

    private void OnEnable()
    {
        if (!isOnCardInspectEventBinded)
        {
            BindOnCardInspectEvent();
        }
    }

    private void OnDisable()
    {
        if(isOnCardInspectEventBinded)
        {
            UnbindOnCardInspectEvent();
        }
    }

    private void Start()
    {
        inspectUI.SetActive(false);
    }

    public void HandleCardInspected(UpgradeDefinitionSO upgradeSO)
    {
        UpdateUI(upgradeSO);
        inspectUI.SetActive(true);
    }

    #region Internal method
    private void UpdateUI(UpgradeDefinitionSO upgradeSO)
    {
        upgradeImage.sprite = upgradeSO.upgradeSprite;
        upgradeName.text = upgradeSO.upgradeName;
        upgradeDescription.text = upgradeSO.upgradeDescription;
    }
    #endregion

    public void BindOnCardInspectEvent()
    {
        InspectComponent.OnCardInspectedEvent += HandleCardInspected;
        isOnCardInspectEventBinded = true;
    }

    public void UnbindOnCardInspectEvent()
    {
        InspectComponent.OnCardInspectedEvent -= HandleCardInspected;
        isOnCardInspectEventBinded = false;
    }
}
