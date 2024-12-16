using System;
using PlayerCore.PlayerComponents;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UserInterface.AbilityInput
{
    public class ChoiceSelectionPanel : MonoBehaviour
    {
        [Header("ChoiceSelection Config")]
        [SerializeField] private TextMeshProUGUI confirmationTitleText;
        [SerializeField] private TextMeshProUGUI confirmationDescriptionText;
        [SerializeField] private Button confirmButton;
        
        [HideInInspector]
        public GameChoice selectedGameChoice;
        
        private UnityAction cachedPreviousEvent;

        public void Initialize(FAbilityInputPanelData data)
        {
            confirmationTitleText.text = data.title;
            confirmationDescriptionText.text = data.description;

            if (cachedPreviousEvent != null)
            {
                confirmButton.onClick.RemoveListener(cachedPreviousEvent);
            }
            
            //add listener if payload is not null, update cachedEvent
            if (data.onConfirmEvent != null)
            {
                cachedPreviousEvent = () => data.onConfirmEvent.Invoke();
                confirmButton.onClick.AddListener(cachedPreviousEvent);            
            }
            else
            {
                //default cachedEvent to null
                cachedPreviousEvent = null;
            }
        }
        
        private void SetCachedGameChoice(GameChoice gameChoice)
        {
            selectedGameChoice = gameChoice;
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