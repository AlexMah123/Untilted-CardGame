using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.LoadoutSelection;
using GameCore.SaveSystem;
using GameCore.SaveSystem.Data;
using LevelCore.LevelConfig.Base;
using LevelCore.LevelManager;
using UnityEngine;
using PlayerCore;
using PlayerCore.Upgrades.Base;
using PlayerCore.Upgrades.UpgradeFactory;
using UserInterface.Cards.LoadoutCard;

namespace UserInterface.GameLoadout
{
    public class GameLoadoutController : MonoBehaviour, ISavableData
    {
        [Header("PlayerUpgrades Parent")]
        [SerializeField] GameObject humanPlayerUpgradesParent;
        [SerializeField] GameObject computerPlayerUpgradesParent;

        List<LoadoutCardUI> humanPlayerUpgrades;
        List<LoadoutCardUI> computerPlayerUpgrades;

        public event Action OnSaveDataLoaded;

        private void OnEnable()
        {
            humanPlayerUpgrades = humanPlayerUpgradesParent.GetComponentsInChildren<LoadoutCardUI>(includeInactive: true).ToList();
            computerPlayerUpgrades = computerPlayerUpgradesParent.GetComponentsInChildren<LoadoutCardUI>(includeInactive: true).ToList();

            foreach (LoadoutCardUI loadoutCard in humanPlayerUpgrades)
            {
                loadoutCard.OnCardClicked += HandleActivateSkill;
            }
        }

        private void OnDisable()
        {
            if (humanPlayerUpgradesParent != null)
            {
                humanPlayerUpgrades = humanPlayerUpgradesParent.GetComponentsInChildren<LoadoutCardUI>(includeInactive: true).ToList();

                foreach (LoadoutCardUI loadoutCard in humanPlayerUpgrades)
                {
                    if (loadoutCard != null)
                    {
                        loadoutCard.OnCardClicked -= HandleActivateSkill;
                    }
                }
            }
        }

        private void Awake()
        {

        }

        private void HandleActivateSkill(FLoadoutCardObj info)
        {
            Debug.Log("Card Clicked");
        }

        private void LoadPlayersUpgrades(GameData data)
        {
            //#TODO: need to check limit for player (probably 5)

            LevelConfigSO currentLevelConfig = LevelDataManager.Instance.currentSelectedLevelSO;

            List<UpgradeType> playerEquippedUpgradesList = data.playerEquippedUpgrades.ToList();

            //for each upgrade equipped by player (save data), display it
            for (int i = 0; i < playerEquippedUpgradesList.Count; i++)
            {
                var createdUpgrade = UpgradeSOFactory.CreateUpgradeDefinitionSO(playerEquippedUpgradesList[i]);

                humanPlayerUpgrades[i].gameObject.SetActive(true);
                humanPlayerUpgrades[i].InitializeCard(new(createdUpgrade));
            }

            //Enemy==============================================================================================================

            FPlayerData computerFPlayerData = currentLevelConfig.aiFPlayer;
            List<UpgradeDefinitionSO> computerPlayerEquippedUpgradesList = computerFPlayerData.upgradesEquipped;

            //for each upgrade equipped by enemy (level config), display it
            for (int i = 0; i < computerPlayerEquippedUpgradesList.Count; i++)
            {
                var createdUpgrade = UpgradeSOFactory.CreateUpgradeDefinitionSO(computerPlayerEquippedUpgradesList[i].upgradeType);

                computerPlayerUpgrades[i].gameObject.SetActive(true);
                computerPlayerUpgrades[i].InitializeCard(new(createdUpgrade));
            }
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
        #endregion
    }
}
