using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using PlayerCore.Upgrades.AbilityInputData;
using PlayerCore.Upgrades.Base;
using PlayerCore.Upgrades.UpgradeFactory;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UserInterface.Cards.LoadoutCard;

namespace UserInterface.AbilityInput
{
    public class TargetUpgradeSelectionPanel : MonoBehaviour, IAbilityInputPanel
    {
        [Header("UpgradeSelection Config")] 
        [SerializeField] GameObject upgradesContainer;
        [SerializeField] private TextMeshProUGUI confirmationTitleText;
        [SerializeField] private TextMeshProUGUI confirmationDescriptionText;
        [SerializeField] private Button confirmButton;
        
        [Header("Current Selected")]
        public UpgradeType selectedUpgrade;
        
        private List<LoadoutCardDisplayUI> playerUpgrades;
        private UnityAction cachedPreviousEvent;

        public void Initialize(FAbilityInputData data)
        {
            confirmationTitleText.text = data.abilityInputTitle;
            confirmationDescriptionText.text = GetActivationDescription(data.upgrade);

            if (cachedPreviousEvent != null)
            {
                confirmButton.onClick.RemoveListener(cachedPreviousEvent);
            }
            
            //add listener if payload is not null, update cachedEvent
            //onConfirm event is passed in type targetUpgradeInput.
            if (data.onConfirmEvent != null)
            {
                cachedPreviousEvent = () => data.onConfirmEvent.Invoke(data.upgrade, new TargetUpgradeInputData(selectedUpgrade));
                confirmButton.onClick.AddListener(cachedPreviousEvent);            
            }
            else
            {
                //default cachedEvent to null
                cachedPreviousEvent = null;
            }
            
            confirmButton.enabled = false;
        }
        
        public string GetActivationDescription(UpgradeDefinitionSO upgrade)
        {
            return selectedUpgrade == UpgradeType.None 
                ? $"You are activating {upgrade.upgradeName}.\nChoose a target upgrade." 
                : $"You are activating {upgrade.upgradeName} with the following choice: {selectedUpgrade.ToString()}.\nAre you sure?";
        }
        
        private void SetCachedGameChoice(UpgradeType upgradeType)
        {
            selectedUpgrade = upgradeType;

            
            if (confirmButton.enabled == false && selectedUpgrade != UpgradeType.None)
            {
                confirmButton.enabled = true;
            }
        }
        
        /*private void CreateChoiceCards()
        {
            if (choiceContainer.transform.childCount == 0)
            {
                foreach (GameChoice choice in Enum.GetValues(typeof(GameChoice)))
                {
                    if(choice == GameChoice.None) continue;
                    
                    FChoiceCardCreation creation = new(choice, choiceContainer.transform);
                    GameObject cardUIGO = choiceCardFactory.CreateCard(creation);
                    ChoiceCardUI cardUI = cardUIGO.GetComponent<ChoiceCardUI>();

                    //Initialise the UI's values
                    cardUI.InitialiseCard(choice, true, isInteractable: false);
                    cardUI.shouldEnlargeOnHover = false;
                    cardUI.shouldReorderToTop = false;
                    cardUI.rectTransform.sizeDelta = cardSize;
                }
            }
        }*/
        

        private void OnDisable()
        {
            confirmButton.onClick.RemoveAllListeners();
            cachedPreviousEvent = null;
            confirmationTitleText.text = String.Empty;
            confirmationDescriptionText.text = String.Empty;
            selectedUpgrade = UpgradeType.None;
        }
        
        
    }
    
}