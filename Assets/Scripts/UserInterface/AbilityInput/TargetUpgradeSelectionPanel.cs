using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using PlayerCore.Upgrades.UpgradeFactory;
using UnityEngine;
using UnityEngine.UI;
using UserInterface.Cards.LoadoutCard;

namespace UserInterface.AbilityInput
{
    public class TargetUpgradeSelectionPanel : MonoBehaviour
    {
        [Header("UpgradeSelection Config")] 
        public GameObject upgradesToChooseFrom;
        public Button confirmButton;
        
        [HideInInspector]
        public UpgradeType selectedUpgrade;
        

        private GameManager gameManager => GameManager.Instance;
        private List<LoadoutCardDisplayUI> playerUpgrades;
        
        
        private void Awake()
        {
            //playerUpgrades = upgradesToChooseFrom.GetComponentsInChildren<LoadoutCardDisplayUI>(includeInactive: true).ToList();
        }

        private void SetCachedGameChoice(UpgradeType upgradeType)
        {
            
        }
    }
}