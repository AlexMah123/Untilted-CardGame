using System;
using System.Collections;
using PlayerCore.PlayerComponents;
using PlayerCore.Upgrades.AbilityInputData;
using PlayerCore.Upgrades.Base;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UserInterface.Cards.ChoiceCard;

namespace UserInterface.AbilityInput
{
    public class ChoiceSelectionPanel : MonoBehaviour, IAbilityInputPanel
    {
        [Header("ChoiceSelection Config")] 
        [SerializeField] private TextMeshProUGUI confirmationTitleText;
        [SerializeField] private TextMeshProUGUI confirmationDescriptionText;
        [SerializeField] private Button confirmButton;
        
        [Header("Current Selected")]
        public GameChoice selectedGameChoice = GameChoice.None;
        
        private Outline selectedCardOutline;
        private UnityAction cachedPreviousEvent;
        private FAbilityInputData cachedInputData;

        public void Initialize(FAbilityInputData data)
        {
            cachedInputData = data;
            UpdatePanelText(cachedInputData.abilityInputTitle, GetActivationDescription(cachedInputData.upgrade));
            
            confirmButton.interactable = false;
        }
        
        public string GetActivationDescription(UpgradeDefinitionSO upgrade)
        {
            return selectedGameChoice == GameChoice.None 
                ? $"You are activating {upgrade.upgradeName}.\nChoose a hand style." 
                : $"You are activating {upgrade.upgradeName} with the following choice: {selectedGameChoice.ToString()}.\nAre you sure?";
        }
        
        public void SetSelectedGameChoice(int enumValue)
        {
            selectedGameChoice = (GameChoice) enumValue;
            UpdatePanelText(cachedInputData.abilityInputTitle, GetActivationDescription(cachedInputData.upgrade));
            
            if (confirmButton.interactable == false && selectedGameChoice != GameChoice.None)
            {
                confirmButton.interactable = true;
            }
            
            UpdateSelectionOutline();
            BindConfirmAbility();
        }

        private void UpdatePanelText(string title, string description)
        {
            confirmationTitleText.text = title;
            confirmationDescriptionText.text = description;
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetChild(0).GetComponent<RectTransform>());
        }

        private void BindConfirmAbility()
        {
            if (cachedPreviousEvent != null)
            {
                confirmButton.onClick.RemoveAllListeners();
            }

            //add listener if payload is not null, update cachedEvent
            //onConfirm event is passed in type choiceSealInput.
            if (cachedInputData.onConfirmEvent != null)
            {
                cachedPreviousEvent = () =>
                {
                    cachedInputData.onConfirmEvent.Invoke(cachedInputData.upgrade, new ChoiceSealInputData(selectedGameChoice));
                    StartCoroutine(DeactivateAfterFrameDelay());
                };
                confirmButton.onClick.AddListener(cachedPreviousEvent);
            }
            else
            {
                //default cachedEvent to null
                cachedPreviousEvent = null;
            }
        }

        private void UpdateSelectionOutline()
        {
            //disable previously selected, assign the new selected and enable that.
            if(selectedCardOutline != null) selectedCardOutline.enabled = false;
            selectedCardOutline = EventSystem.current.currentSelectedGameObject.transform.parent.GetComponent<Outline>();
            selectedCardOutline.enabled = true;
        }
        
        private void OnDisable()
        {
            if(selectedCardOutline != null) selectedCardOutline.enabled = false;
            confirmButton.onClick.RemoveAllListeners();
            cachedPreviousEvent = null;
            confirmationTitleText.text = String.Empty;
            confirmationDescriptionText.text = String.Empty;
            selectedGameChoice = GameChoice.None;
        }

        private IEnumerator DeactivateAfterFrameDelay()
        {
            yield return new WaitForEndOfFrame();

            gameObject.SetActive(false);
        }
    }
}