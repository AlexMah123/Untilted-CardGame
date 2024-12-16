using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using PlayerCore.Upgrades.UpgradeFactory;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UserInterface.Cards.LoadoutCard;

namespace UserInterface.AbilityInput
{
    public class TargetUpgradeSelectionPanel : MonoBehaviour
    {
        [Header("UpgradeSelection Config")] 
        public GameObject upgradesToChooseFrom;
        [SerializeField] private TextMeshProUGUI confirmationTitleText;
        [SerializeField] private TextMeshProUGUI confirmationDescriptionText;
        [SerializeField] private Button confirmButton;
        
        [HideInInspector]
        public UpgradeType selectedUpgrade;
        
        private GameManager gameManager => GameManager.Instance;
        private List<LoadoutCardDisplayUI> playerUpgrades;
        
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
        
        private void SetCachedGameChoice(UpgradeType upgradeType)
        {
            
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