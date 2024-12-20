using System.Collections.Generic;
using System.Linq;
using GameCore;
using GameCore.LoadoutSelection;
using PlayerCore;
using PlayerCore.Upgrades.Base;
using PlayerCore.Upgrades.UpgradeFactory;
using UnityEngine;
using UserInterface.AbilityInput;
using UserInterface.Cards.LoadoutCard;

namespace UserInterface.Gameplay.GameLoadout
{
    public class GameLoadoutController : MonoBehaviour
    {
        [Header("PlayerUpgrades Parent")] 
        [SerializeField] private GameObject playerUpgradesParent;
        [SerializeField] private GameObject aiPlayerUpgradesParent;

        List<LoadoutCardGameplayUI> playerUpgrades;
        List<LoadoutCardGameplayUI> aiPlayerUpgrades;

        private Player Player => GameManager.Instance.player;
        private Player AIPlayer => GameManager.Instance.aiPlayer;

        private AbilityInputManager abilityInputManager;
        
        private void OnEnable()
        {
            LoadAndBindCardOnClick();
        }
        
        private void OnDisable()
        {
            UnbindCardOnClick();
            UnbindPlayerLoadoutComponent();
        }

        private void Awake()
        {
            abilityInputManager = GetComponent<AbilityInputManager>();
            
            if(abilityInputManager == null) Debug.LogError("AbilityInputManager is missing");
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

        private void HandleAbilityConfirmation(FLoadoutCardObj cardObjInfo)
        {
            abilityInputManager.PromptForConfirmation(cardObjInfo, Player.ActiveLoadoutComponent.HandleActivateSkill);
        }

        private void ResetPlayerLoadoutUI()
        {
            foreach (LoadoutCardGameplayUI loadoutCard in playerUpgrades)
            {
                loadoutCard.gameObject.SetActive(false);
            }
        }

        private void ResetAIPlayerLoadout()
        {
            foreach (LoadoutCardGameplayUI loadoutCard in aiPlayerUpgrades)
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

        #region Bind CardOnClick
        private void LoadAndBindCardOnClick()
        {
            if (playerUpgradesParent == null || aiPlayerUpgradesParent == null) Debug.LogError("Player/EnemyUpgradeParent is missing");
                
            playerUpgrades = playerUpgradesParent.GetComponentsInChildren<LoadoutCardGameplayUI>(includeInactive: true)
                .ToList();
            aiPlayerUpgrades = aiPlayerUpgradesParent.GetComponentsInChildren<LoadoutCardGameplayUI>(includeInactive: true)
                .ToList();

            foreach (LoadoutCardGameplayUI loadoutCard in playerUpgrades)
            {
                loadoutCard.OnCardClicked += HandleAbilityConfirmation;
            }
        }
        
        private void UnbindCardOnClick()
        {
            if (playerUpgradesParent != null)
            {
                playerUpgrades = playerUpgradesParent.GetComponentsInChildren<LoadoutCardGameplayUI>(includeInactive: true)
                    .ToList();

                foreach (LoadoutCardGameplayUI loadoutCard in playerUpgrades)
                {
                    if (loadoutCard != null)
                    {
                        loadoutCard.OnCardClicked -= HandleAbilityConfirmation;
                    }
                }
            }
        }
        

        #endregion
    }
}