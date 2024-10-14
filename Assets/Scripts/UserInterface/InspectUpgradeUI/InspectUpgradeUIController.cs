using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Upgrades.Base;

namespace UserInterface.InspectUpgradeUI
{
    public class InspectUpgradeUIController : MonoBehaviour
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
            if (isOnCardInspectEventBinded)
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
            InspectComponent.OnCardInspected += HandleCardInspected;
            isOnCardInspectEventBinded = true;
        }

        public void UnbindOnCardInspectEvent()
        {
            InspectComponent.OnCardInspected -= HandleCardInspected;
            isOnCardInspectEventBinded = false;
        }
    }
}
