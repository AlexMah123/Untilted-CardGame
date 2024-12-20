using System;
using GameCore.LoadoutSelection;
using PlayerCore.PlayerComponents;
using PlayerCore.Upgrades.AbilityInputData;
using PlayerCore.Upgrades.Base;
using PlayerCore.Upgrades.UpgradeFactory;
using UnityEngine;

namespace UserInterface.AbilityInput
{
    public interface IAbilityInputPanel
    {
        void Initialize(FAbilityInputData data);
        string GetActivationDescription(UpgradeDefinitionSO upgrade);

    }
    
    public struct FAbilityInputData
    {
        public string abilityInputTitle;
        public UpgradeDefinitionSO upgrade;
        public Action<UpgradeDefinitionSO, IAbilityInputData> onConfirmEvent;
        
        public FAbilityInputData( string abilityInputTitle, UpgradeDefinitionSO upgrade, Action<UpgradeDefinitionSO, IAbilityInputData> onConfirmEvent)
        {
            this.abilityInputTitle = abilityInputTitle;
            this.upgrade = upgrade;
            this.onConfirmEvent = onConfirmEvent;
        }
    }
    
    public class AbilityInputManager : MonoBehaviour
    {
        [Header("UI Panels")]
        [SerializeField] private GameObject confirmationPanel;
        [SerializeField] private GameObject choiceSelectionPanel;
        [SerializeField] private GameObject targetUpgradePanel;
        
        private IAbilityInputPanel confirmationInputPanel;
        private IAbilityInputPanel choiceSelectionInputPanel;
        private IAbilityInputPanel targetUpgradeInputPanel;
        
        private const string ACTIVATIONPROMPT = "Ability Activation";

        private void Awake()
        {
            confirmationInputPanel = confirmationPanel.GetComponent<ConfirmationPanelManager>();
            choiceSelectionInputPanel = choiceSelectionPanel.GetComponent<ChoiceSelectionPanel>();
            targetUpgradeInputPanel = targetUpgradePanel.GetComponent<TargetUpgradeSelectionPanel>();
            
            if(confirmationPanel == null) Debug.LogError("Confirmation Panel Manager is null");
            if(choiceSelectionPanel == null) Debug.LogError("Choice Selection Panel Manager is null");
            if(targetUpgradePanel == null) Debug.LogError("Target Upgrade Selection Panel Manager is null");
        }
        
        public void PromptForConfirmation(FLoadoutCardObj cardObjInfo, Action<UpgradeDefinitionSO, IAbilityInputData> onAbilityConfirmed)
        {
            //Handle relevant processing for which prompt to show, and call the callback
            //Get the relevant confirmationPayload and initialize confirmationPanel.
            FAbilityInputData abilityInputPayload = new FAbilityInputData(ACTIVATIONPROMPT, cardObjInfo.upgradeSO, onAbilityConfirmed);

            switch (cardObjInfo.upgradeSO.abilityConfirmationType)
            {
                case AbilityConfirmationType.ChoiceSeal:

                    choiceSelectionInputPanel.Initialize(abilityInputPayload);
                    choiceSelectionPanel.SetActive(true);
                    break;
                
                case AbilityConfirmationType.TargetUpgrade:

                    targetUpgradeInputPanel.Initialize(abilityInputPayload);
                    targetUpgradePanel.SetActive(true);
                    break;
                
                default:
                    confirmationInputPanel.Initialize(abilityInputPayload);
                    confirmationPanel.SetActive(true); 
                    break;
            }
        }
    }
}