using System;
using System.Collections.Generic;
using System.Linq;
using LevelManager;
using LoadoutSelection;
using PlayerCore;
using SaveSystem;
using SaveSystem.Data;
using UnityEngine;
using UnityEngine.UI;
using Upgrades.Base;
using UserInterface.Cards.LoadoutCard;

namespace RewardSystem
{
    public class RewardSystemController : MonoBehaviour, ISavableData
    {
        [Header("RewardUI Config")] [SerializeField]
        LoadoutCardUI rewardPrefab;

        [Header("Reward Display Config")] [SerializeField]
        RectTransform rewardContainer;
        [SerializeField] RectTransform content;
        [SerializeField] GridLayoutGroup gridLayout;
        [SerializeField] float rewardContainerXScale = 1.5f;
        [SerializeField] float rewardContainerYScale = 1f;


        [Header("Runtime Data")]
        [SerializeField] List<UpgradeDefinitionSO> upgradeRewardList;
        [SerializeField] PlayerStats playerRewardStats;
    
        //cached
        List<LoadoutCardUI> rewardUIList;

        //ISavableData
        public event Action OnSaveDataLoaded;

        private void Start()
        {
            if (LevelDataManager.Instance.currentSelectedLevelSO == null)
            {
                Debug.LogWarning("LevelManager does not exist, currentSelectedLevelSO is null");
                return;
            }

            //Based on the level, there is a set rewardList
            upgradeRewardList = LevelDataManager.Instance.currentSelectedLevelSO.rewardList;
            playerRewardStats = LevelDataManager.Instance.currentSelectedLevelSO.rewardStats;
        
            InitializeRewardContainer();
            UpdateRewardDisplay();

            //save the rewards to the player's save data
            SaveSystemManager.Instance.SaveGame();
        }

        private void InitializeRewardContainer()
        {
            //initialize the preset amount of rewardUI
            rewardUIList = content.GetComponentsInChildren<LoadoutCardUI>(includeInactive: true).ToList();
        }

        private void UpdateRewardDisplay()
        {
            //reset all rewardUI
            foreach (LoadoutCardUI reward in rewardUIList)
            {
                reward.gameObject.SetActive(false);
            }

            //based on rewardList, display that many reward UI
            foreach (var upgrade in upgradeRewardList)
            {
                LoadoutCardUI rewardUI = GetAvailableRewardUI();

                if (rewardUI)
                {
                    //populate the data 
                    rewardUI.InitializeCard(new LoadoutCardGOInfo(upgrade));
                    rewardUI.gameObject.SetActive(true);
                }
                else
                {
                    Debug.LogError("failed to create rewardUI");
                    break;
                }
            }

            AdjustRewardContainerSize(upgradeRewardList.Count);
        }

        private LoadoutCardUI GetAvailableRewardUI()
        {
            LoadoutCardUI availableRewardUI = rewardUIList.FirstOrDefault(x => x.gameObject.activeInHierarchy == false);
            bool failedToGetExistingReward = availableRewardUI == null;

            if (failedToGetExistingReward)
            {
                availableRewardUI = Instantiate(rewardPrefab, content);
                rewardUIList.Add(availableRewardUI);
            }

            return availableRewardUI;
        }

        private void AdjustRewardContainerSize(int rewardCount)
        {
            if (rewardCount == 0)
            {
                // If no rewards, hide the reward container
                rewardContainer.sizeDelta = new Vector2(0, 0);
            }
            else
            {
                //force update the layout
                LayoutRebuilder.ForceRebuildLayoutImmediate(content);

                Vector2 preferredSize = content.GetComponent<RectTransform>().sizeDelta;

                float totalWidth = preferredSize.x / rewardContainerXScale;
                float totalHeight = preferredSize.y / rewardContainerYScale;

                rewardContainer.sizeDelta = new Vector2(totalWidth, totalHeight);
            }
        }


        public void LoadData(GameData data)
        {
        
        }

        public void SaveData(ref GameData data)
        {
            SaveUpgradesToPlayerData(ref data);
        }

        private void SaveUpgradesToPlayerData(ref GameData data)
        {
            //save the cardUpgrades to the player's save data'
            foreach (var upgrades in upgradeRewardList)
            {
                data.playerUnlockedUpgrades.Add(upgrades.upgradeType);
            }
        
            //update the player's stats'
            data.upgradedPlayerStats.health += playerRewardStats.health;
            data.upgradedPlayerStats.damage += playerRewardStats.damage;
            data.upgradedPlayerStats.cardSlots += playerRewardStats.cardSlots;
            data.upgradedPlayerStats.energy += playerRewardStats.energy;
        }
    }
}