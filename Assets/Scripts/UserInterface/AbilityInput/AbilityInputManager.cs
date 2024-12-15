using System;
using GameCore.LoadoutSelection;
using PlayerCore.PlayerComponents;
using PlayerCore.Upgrades.AbilityInputData;
using PlayerCore.Upgrades.Base;
using PlayerCore.Upgrades.UpgradeFactory;
using UnityEngine;

namespace UserInterface.AbilityInput
{
    public class AbilityInputManager : MonoBehaviour
    {
        [Header("UI Panels")]
        [SerializeField] private ConfirmationPanelManager confirmationPanel;
        [SerializeField] private ChoiceSelectionPanel choiceSelectionPanel;
        [SerializeField] private TargetUpgradeSelectionPanel targetUpgradePanel;
        
        private const string ACTIVATIONPROMPT = "Ability Activation";
        
        private GameChoice cachedGameChoice;
        private UpgradeType cachedUpgradeType;

        private void Awake()
        {
            confirmationPanel = confirmationPanel.GetComponent<ConfirmationPanelManager>();
            
            if(confirmationPanel == null) Debug.LogError("Confirmation Panel Manager is null");
        }
        
        public void PromptForConfirmation(FLoadoutCardObj cardObjInfo, Action<UpgradeDefinitionSO, IAbilityInputData> onAbilityConfirmed)
        {
            //Handle relevant processing for which prompt to show, and call the callback
            switch (cardObjInfo.upgradeSO.abilityConfirmationType)
            {
                case AbilityConfirmationType.ChoiceSeal:
                    choiceSelectionPanel.confirmButton.onClick.AddListener(() =>
                    { 
                        cachedGameChoice = choiceSelectionPanel.selectedGameChoice;
                    });
                    
                    choiceSelectionPanel.gameObject.SetActive(true);
                    break;
                
                case AbilityConfirmationType.TargetUpgrade:
                    targetUpgradePanel.confirmButton.onClick.AddListener(() =>
                    {
                        cachedUpgradeType = targetUpgradePanel.selectedUpgrade;
                    });
                    
                    targetUpgradePanel.gameObject.SetActive(true);
                    break;
                
                default:
                    confirmationPanel.gameObject.SetActive(true); 
                    break;
            }

            //Get the relevant confirmationPayload and initialize confirmationPanel.
            FConfirmationPanelData confirmationPayload = GetConfirmationPayload(cardObjInfo.upgradeSO, cardObjInfo.upgradeSO.abilityConfirmationType, onAbilityConfirmed);
            confirmationPanel.Initialize(confirmationPayload);
        }

        private FConfirmationPanelData GetConfirmationPayload(UpgradeDefinitionSO targetAbility, AbilityConfirmationType confirmationType, Action<UpgradeDefinitionSO, IAbilityInputData> onAbilityConfirmed)
        {
            Action onConfirmCallback;
            
            switch (confirmationType)
            {
                case AbilityConfirmationType.ChoiceSeal:
                    onConfirmCallback = () =>
                        onAbilityConfirmed?.Invoke(targetAbility, new ChoiceSealInputData(cachedGameChoice));
                    break;

                case AbilityConfirmationType.TargetUpgrade:
                    onConfirmCallback = () =>
                        onAbilityConfirmed?.Invoke(targetAbility, new TargetUpgradeInputData(cachedUpgradeType));
                    break;

                default:
                    onConfirmCallback = () =>
                        onAbilityConfirmed?.Invoke(targetAbility, null);
                    break;
            }

            return new FConfirmationPanelData(
                ACTIVATIONPROMPT,
                GetActivationDescription(targetAbility, null),
                onConfirmCallback
            );
        }

        private string GetActivationDescription(UpgradeDefinitionSO upgrade, IAbilityInputData inputData)
        {
            string inputDescription = inputData != null ? inputData.ToString() : string.Empty;
            string inputChoice = inputData != null ? $"with the following choice: {inputDescription}" : string.Empty;

            return $"You are activating {upgrade.upgradeName} {inputChoice}\nAre you sure?";
        }
    }
}