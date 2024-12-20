using System;
using PlayerCore.Upgrades.AbilityInputData;
using PlayerCore.Upgrades.Base;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UserInterface.AbilityInput
{
    public class ConfirmationPanelManager : MonoBehaviour, IAbilityInputPanel
    {
        [Header("Confirmation Config")]
        [SerializeField] private TextMeshProUGUI confirmationTitleText;
        [SerializeField] private TextMeshProUGUI confirmationDescriptionText;
        [SerializeField] private Button confirmButton;
        
        private UnityAction cachedPreviousEvent;

        public void Initialize(FAbilityInputData data)
        {
            confirmationTitleText.text = data.abilityInputTitle;
            confirmationDescriptionText.text = GetActivationDescription(data.upgrade);

            if (cachedPreviousEvent != null)
            {
                confirmButton.onClick.RemoveListener(cachedPreviousEvent);
            }
            
            //add listener if payload is not null, update cachedEvent.
            //onConfirm event is passed in null since it is generic
            if (data.onConfirmEvent != null)
            {
                cachedPreviousEvent = () => data.onConfirmEvent.Invoke(data.upgrade, null);
                confirmButton.onClick.AddListener(cachedPreviousEvent);            
            }
            else
            {
                //default cachedEvent to null
                cachedPreviousEvent = null;
            }
        }
        
        public string GetActivationDescription(UpgradeDefinitionSO upgrade)
        {
            return $"You are activating {upgrade.upgradeName}.\nAre you sure?";
        }

        private void OnDisable()
        {
            confirmButton.onClick.RemoveAllListeners();
            cachedPreviousEvent = null;
            confirmationTitleText.text = String.Empty;
            confirmationDescriptionText.text = String.Empty;
        }
    }
}