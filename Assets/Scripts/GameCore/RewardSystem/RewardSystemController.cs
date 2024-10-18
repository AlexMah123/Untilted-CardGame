using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameCore.LoadoutSelection;
using GameCore.SaveSystem;
using GameCore.SaveSystem.Data;
using LevelCore.LevelManager;
using PlayerCore;
using PlayerCore.Upgrades.Base;
using UnityEngine;
using UnityEngine.UI;
using UserInterface.Cards.LoadoutCard;

namespace GameCore.RewardSystem
{
    public class RewardSystemController : MonoBehaviour, ISavableData
    {
        [Header("RewardUI Config")] [SerializeField]
        LoadoutCardUI rewardPrefab;

        [Header("Reward Display Ref")] 
        [SerializeField] private RectTransform rewardContainer;
        [SerializeField] private RectTransform content;
        [SerializeField] private GridLayoutGroup gridLayout;
        
        [Header("Reward Display Config")]
        [SerializeField] private float rewardContainerXScale = 1.5f;
        [SerializeField] private float rewardContainerYScale = 1f;
        [SerializeField] private bool immediateUpdate = false;
        [SerializeField] private float rewardDisplayDelay = 0.25f;
        [SerializeField] private float popupScale = 1.2f;

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

            if (immediateUpdate)
            {
                UpdateRewardDisplay();
            }
            else
            { 
                StartCoroutine(ShowRewardSequantially());
            }
            
            

            //save the rewards to the player's save data
            SaveSystemManager.Instance.SaveGame();
        }

        private void InitializeRewardContainer()
        {
            //initialize the preset amount of rewardUI
            rewardUIList = content.GetComponentsInChildren<LoadoutCardUI>(includeInactive: true).ToList();
        }
        
        private IEnumerator ShowRewardSequantially()
        {
            yield return new WaitForSeconds(0.1f);
            
            // Reset all rewardUI
            foreach (LoadoutCardUI reward in rewardUIList)
            {
                reward.gameObject.SetActive(false);
            }

            // Based on rewardList, display the rewards sequentially with a delay
            foreach (var upgrade in upgradeRewardList)
            {
                LoadoutCardUI rewardUI = GetAvailableRewardUI();

                if (rewardUI)
                {
                    // Populate the data
                    rewardUI.InitializeCard(new FLoadoutCardObj(upgrade));
                    rewardUI.gameObject.SetActive(true);
                    rewardUI.gameObject.transform.localScale = new Vector3(popupScale, popupScale, 1);
                    
                    yield return new WaitForSeconds(rewardDisplayDelay); // Delay before showing the next reward
                    rewardUI.gameObject.transform.localScale = Vector3.one;
                }
                else
                {
                    Debug.LogError("failed to create rewardUI");
                    break;
                }
            }

            AdjustRewardContainerSize(upgradeRewardList.Count);
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
                    rewardUI.InitializeCard(new FLoadoutCardObj(upgrade));
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