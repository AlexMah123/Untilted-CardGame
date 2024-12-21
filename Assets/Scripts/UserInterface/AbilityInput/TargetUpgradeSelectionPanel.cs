using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using PlayerCore.Upgrades.AbilityInputData;
using PlayerCore.Upgrades.Base;
using PlayerCore.Upgrades.UpgradeFactory;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UserInterface.AbilityInput.Factory;
using UserInterface.Cards.LoadoutCard;

namespace UserInterface.AbilityInput
{
    public class TargetUpgradeSelectionPanel : MonoBehaviour, IAbilityInputPanel
    {
        [Header("UpgradeSelection Config")] 
        [SerializeField] private AbilityInputButtonFactory abilityButtonFactory;
        [SerializeField] private GameObject upgradePrefab;
        [SerializeField] private GameObject upgradesContainer;
        
        [SerializeField] private TextMeshProUGUI confirmationTitleText;
        [SerializeField] private TextMeshProUGUI confirmationDescriptionText;
        [SerializeField] private Button confirmButton;
        
        [Header("Current Selected")]
        public UpgradeType selectedUpgrade = UpgradeType.None;
        
        private Outline selectedCardOutline;
        private List<GameObject> upgradeObjectList = new List<GameObject>();
        private List<UpgradeDefinitionSO> targetPlayerTotalUpgrades = new List<UpgradeDefinitionSO>();
        
        private UnityAction cachedPreviousEvent;
        private FAbilityInputData cachedInputData;
        private static GameManager GameManager => GameManager.Instance;

        private void Awake()
        {
            if(abilityButtonFactory == null) abilityButtonFactory = gameObject.AddComponent<AbilityInputButtonFactory>();
            if(upgradePrefab == null) Debug.LogWarning("No upgrade prefab assigned!");
            if(upgradesContainer == null) Debug.LogWarning("No upgrades container assigned!");
        }

        private void SetupPanel()
        {
            //reset and reassign the enemyPlayersData
            targetPlayerTotalUpgrades.Clear();
            upgradeObjectList.Clear();
            targetPlayerTotalUpgrades = GameManager.aiPlayer.ActiveLoadoutComponent.cardUpgradeList;

            foreach (UpgradeDefinitionSO upgrade in targetPlayerTotalUpgrades)
            {
                GameObject upgradeObject = 
                    abilityButtonFactory.CreateAbilityInputButton(upgradePrefab, upgrade, upgradesContainer.transform, SetCachedGameChoice);
                upgradeObjectList.Add(upgradeObject);
            }
        }
        
        public void Initialize(FAbilityInputData data)
        {
            cachedInputData = data;
            UpdatePanelText(cachedInputData.abilityInputTitle, GetActivationDescription(cachedInputData.upgrade));
            SetupPanel();
            
            confirmButton.interactable = false;
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
            UpdatePanelText(cachedInputData.abilityInputTitle, GetActivationDescription(cachedInputData.upgrade));
            
            if (confirmButton.interactable == false && selectedUpgrade != UpgradeType.None)
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
                confirmButton.onClick.RemoveListener(cachedPreviousEvent);
            }

            //add listener if payload is not null, update cachedEvent
            //onConfirm event is passed in type targetUpgradeInput.
            if (cachedInputData.onConfirmEvent != null)
            {
                cachedPreviousEvent = () =>
                {
                    cachedInputData.onConfirmEvent.Invoke(cachedInputData.upgrade, new TargetUpgradeInputData(selectedUpgrade));
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
            selectedUpgrade = UpgradeType.None;
        }
        
        private IEnumerator DeactivateAfterFrameDelay()
        {
            yield return new WaitForEndOfFrame();

            gameObject.SetActive(false);
        }
    }
    
}