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
        
        //#TODO: MAKE IT SO THE EVENT SUBSCRIBES TO UPDATE THE CACHED CHOICE
        private GameChoice cachedGameChoice = GameChoice.Rock;
        private UpgradeType cachedUpgradeType = UpgradeType.None;

        private void Awake()
        {
            confirmationPanel = confirmationPanel.GetComponent<ConfirmationPanelManager>();
            choiceSelectionPanel = choiceSelectionPanel.GetComponent<ChoiceSelectionPanel>();
            targetUpgradePanel = targetUpgradePanel.GetComponent<TargetUpgradeSelectionPanel>();
            
            if(confirmationPanel == null) Debug.LogError("Confirmation Panel Manager is null");
            if(choiceSelectionPanel == null) Debug.LogError("Choice Selection Panel Manager is null");
            if(targetUpgradePanel == null) Debug.LogError("Target Upgrade Selection Panel Manager is null");
        }
        
        public void PromptForConfirmation(FLoadoutCardObj cardObjInfo, Action<UpgradeDefinitionSO, IAbilityInputData> onAbilityConfirmed)
        {
            //Handle relevant processing for which prompt to show, and call the callback
            //Get the relevant confirmationPayload and initialize confirmationPanel.
            FAbilityInputPanelData abilityInputPayload = GetConfirmationPayload(cardObjInfo.upgradeSO, cardObjInfo.upgradeSO.abilityConfirmationType, onAbilityConfirmed);

            switch (cardObjInfo.upgradeSO.abilityConfirmationType)
            {
                case AbilityConfirmationType.ChoiceSeal:
                    /*choiceSelectionPanel.confirmButton.onClick.AddListener(() =>
                    { 
                        cachedGameChoice = choiceSelectionPanel.selectedGameChoice;
                    });*/
                    choiceSelectionPanel.Initialize(abilityInputPayload);
                    choiceSelectionPanel.gameObject.SetActive(true);
                    break;
                
                case AbilityConfirmationType.TargetUpgrade:
                    /*targetUpgradePanel.confirmButton.onClick.AddListener(() =>
                    {
                        cachedUpgradeType = targetUpgradePanel.selectedUpgrade;
                    });*/
                    targetUpgradePanel.Initialize(abilityInputPayload);
                    targetUpgradePanel.gameObject.SetActive(true);
                    break;
                
                default:
                    confirmationPanel.Initialize(abilityInputPayload);
                    confirmationPanel.gameObject.SetActive(true); 
                    break;
            }
        }

        private FAbilityInputPanelData GetConfirmationPayload(UpgradeDefinitionSO targetAbility, AbilityConfirmationType confirmationType, Action<UpgradeDefinitionSO, IAbilityInputData> onAbilityConfirmed)
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

            return new FAbilityInputPanelData(
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