using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using GameCore.LoadoutSelection;
using PlayerCore;
using PlayerCore.Upgrades.Base;
using PlayerCore.Upgrades.UpgradeFactory;
using UnityEngine;
using UserInterface.Cards.LoadoutCard;

namespace UserInterface.Gameplay.GameLoadout
{
    public class GameLoadoutController : MonoBehaviour
    {
        [Header("PlayerUpgrades Parent")] 
        [SerializeField] private GameObject playerUpgradesParent;

        [SerializeField] private GameObject aiPlayerUpgradesParent;

        List<LoadoutCardUI> playerUpgrades;
        List<LoadoutCardUI> aiPlayerUpgrades;

        private Player Player => GameManager.Instance.player;
        private Player AIPlayer => GameManager.Instance.aiPlayer;

        private void OnEnable()
        {
            playerUpgrades = playerUpgradesParent.GetComponentsInChildren<LoadoutCardUI>(includeInactive: true)
                .ToList();
            aiPlayerUpgrades = aiPlayerUpgradesParent.GetComponentsInChildren<LoadoutCardUI>(includeInactive: true)
                .ToList();

            foreach (LoadoutCardUI loadoutCard in playerUpgrades)
            {
                loadoutCard.OnCardClicked += HandleActivateSkill;
            }
        }

        private void OnDisable()
        {
            if (playerUpgradesParent != null)
            {
                playerUpgrades = playerUpgradesParent.GetComponentsInChildren<LoadoutCardUI>(includeInactive: true)
                    .ToList();

                foreach (LoadoutCardUI loadoutCard in playerUpgrades)
                {
                    if (loadoutCard != null)
                    {
                        loadoutCard.OnCardClicked -= HandleActivateSkill;
                    }
                }
            }

            UnbindPlayerLoadoutComponent();
        }

        private void Start()
        {
            BindPlayersLoadoutComponent();

            HandlePlayerLoadoutUpdate(Player.ActiveLoadoutComponent.cardUpgradeList);
            HandleAIPlayerLoadoutUpdate(AIPlayer.ActiveLoadoutComponent.cardUpgradeList);
        }

        private void HandlePlayerLoadoutUpdate(List<UpgradeDefinitionSO> playerLoadoutList)
        {
            ResetPlayerLoadoutUI();

            //for each upgrade equipped by player, display it
            for (int i = 0; i < playerLoadoutList.Count; i++)
            {
                var createdUpgrade = UpgradeSOFactory.CreateUpgradeDefinitionSO(playerLoadoutList[i].upgradeType);

                playerUpgrades[i].gameObject.SetActive(true);
                playerUpgrades[i].InitializeCard(new(createdUpgrade));
            }
        }

        private void HandleAIPlayerLoadoutUpdate(List<UpgradeDefinitionSO> aiPlayerLoadoutList)
        {
            ResetAIPlayerLoadout();

            //for each upgrade equipped by enemy (level config), display it
            for (int i = 0; i < aiPlayerLoadoutList.Count; i++)
            {
                var createdUpgrade = UpgradeSOFactory.CreateUpgradeDefinitionSO(aiPlayerLoadoutList[i].upgradeType);

                aiPlayerUpgrades[i].gameObject.SetActive(true);
                aiPlayerUpgrades[i].InitializeCard(new(createdUpgrade));
            }
        }

        private void HandleActivateSkill(FLoadoutCardObj info)
        {
            Debug.Log("Card Clicked, Prompt player confirmation to activate skill");

            GameManager.Instance.player.ActiveLoadoutComponent.HandleActivateSkill(info.upgradeSO);
        }

        private void ResetPlayerLoadoutUI()
        {
            foreach (LoadoutCardUI loadoutCard in playerUpgrades)
            {
                loadoutCard.gameObject.SetActive(false);
            }
        }

        private void ResetAIPlayerLoadout()
        {
            foreach (LoadoutCardUI loadoutCard in aiPlayerUpgrades)
            {
                loadoutCard.gameObject.SetActive(false);
            }
        }

        #region Bind PlayerLoadoutComponent

        private void BindPlayersLoadoutComponent()
        {
            if (Player)
            {
                Player.ActiveLoadoutComponent.OnLoadoutChanged += HandlePlayerLoadoutUpdate;
            }

            if (AIPlayer)
            {
                AIPlayer.ActiveLoadoutComponent.OnLoadoutChanged += HandlePlayerLoadoutUpdate;
            }
        }

        private void UnbindPlayerLoadoutComponent()
        {
            if (Player)
            {
                Player.ActiveLoadoutComponent.OnLoadoutChanged -= HandlePlayerLoadoutUpdate;
            }

            if (AIPlayer)
            {
                AIPlayer.ActiveLoadoutComponent.OnLoadoutChanged -= HandlePlayerLoadoutUpdate;
            }
        }

        #endregion
    }
}