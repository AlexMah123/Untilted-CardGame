using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using GameCore.LoadoutSelection;
using GameCore.SaveSystem;
using GameCore.SaveSystem.Data;
using LevelCore.LevelConfig.Base;
using LevelCore.LevelManager;
using PlayerCore;
using PlayerCore.Upgrades.Base;
using PlayerCore.Upgrades.UpgradeFactory;
using UnityEngine;
using UserInterface.Cards.LoadoutCard;

namespace UserInterface.Gameplay.GameLoadout
{
    public class GameLoadoutController : MonoBehaviour, ISavableData
    {
        [Header("PlayerUpgrades Parent")]
        [SerializeField] GameObject playerUpgradesParent;
        [SerializeField] GameObject AIPlayerUpgradesParent;

        List<LoadoutCardUI> playerUpgrades;
        List<LoadoutCardUI> AIPlayerUpgrades;
        
        //savesystem interface
        public event Action OnSaveDataLoaded;

        private void OnEnable()
        {
            playerUpgrades = playerUpgradesParent.GetComponentsInChildren<LoadoutCardUI>(includeInactive: true).ToList();
            AIPlayerUpgrades = AIPlayerUpgradesParent.GetComponentsInChildren<LoadoutCardUI>(includeInactive: true).ToList();

            foreach (LoadoutCardUI loadoutCard in playerUpgrades)
            {
                loadoutCard.OnCardClicked += HandleActivateSkill;
            }
        }

        private void OnDisable()
        {
            if (playerUpgradesParent != null)
            {
                playerUpgrades = playerUpgradesParent.GetComponentsInChildren<LoadoutCardUI>(includeInactive: true).ToList();

                foreach (LoadoutCardUI loadoutCard in playerUpgrades)
                {
                    if (loadoutCard != null)
                    {
                        loadoutCard.OnCardClicked -= HandleActivateSkill;
                    }
                }
            }
        }

        private void UpdateLoadoutUI()
        {
            
        }

        private void HandleActivateSkill(FLoadoutCardObj info)
        {
            Debug.Log("Card Clicked, Prompt player confirmation to activate skill");
            
            GameManager.Instance.humanPlayer.ActiveLoadoutComponent.HandleActivateSkill(info.upgradeSO);
        }
        
        #region Save System Interface
        public void LoadData(GameData data)
        {
            if (LevelDataManager.Instance.currentSelectedLevelSO == null)
            {
                throw new NullReferenceException("LevelDataManager does not have a levelSO selected");
            }

            LoadPlayersUpgrades(data);

            OnSaveDataLoaded?.Invoke();
        }

        public void SaveData(ref GameData data)
        {
        
        }
        
        private void LoadPlayersUpgrades(GameData data)
        {
            foreach (LoadoutCardUI loadoutCard in playerUpgrades)
            {
                loadoutCard.gameObject.SetActive(false);
            }

            foreach (LoadoutCardUI loadoutCard in AIPlayerUpgrades)
            {
                loadoutCard.gameObject.SetActive(false);
            }
            
            //#TODO: need to check limit for player (probably 5)

            LevelConfigSO currentLevelConfig = LevelDataManager.Instance.currentSelectedLevelSO;
            List<UpgradeType> playerEquippedUpgradesList = data.playerEquippedUpgrades.ToList();

            //for each upgrade equipped by player (save data), display it
            for (int i = 0; i < playerEquippedUpgradesList.Count; i++)
            {
                var createdUpgrade = UpgradeSOFactory.CreateUpgradeDefinitionSO(playerEquippedUpgradesList[i]);

                playerUpgrades[i].gameObject.SetActive(true);
                playerUpgrades[i].InitializeCard(new(createdUpgrade));
            }

            //Enemy==============================================================================================================

            FPlayerData computerFPlayerData = currentLevelConfig.aiFPlayer;
            List<UpgradeDefinitionSO> computerPlayerEquippedUpgradesList = computerFPlayerData.upgradesEquipped;

            //for each upgrade equipped by enemy (level config), display it
            for (int i = 0; i < computerPlayerEquippedUpgradesList.Count; i++)
            {
                var createdUpgrade = UpgradeSOFactory.CreateUpgradeDefinitionSO(computerPlayerEquippedUpgradesList[i].upgradeType);

                AIPlayerUpgrades[i].gameObject.SetActive(true);
                AIPlayerUpgrades[i].InitializeCard(new(createdUpgrade));
            }
        }
        #endregion
    }
}
