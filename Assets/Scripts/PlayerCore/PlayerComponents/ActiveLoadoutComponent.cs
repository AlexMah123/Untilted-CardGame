using System;
using System.Collections.Generic;
using GameCore;
using PlayerCore.Upgrades.AbilityInputData;
using PlayerCore.Upgrades.Base;
using PlayerCore.Upgrades.UpgradeFactory;
using UnityEngine;

namespace PlayerCore.PlayerComponents
{
    public class ActiveLoadoutComponent : MonoBehaviour
    {
        //Injected Dependency
        [HideInInspector] public Player enemyPlayer;
        [HideInInspector] public Player attachedPlayer;

        [Header("Runtime Value")] 
        public int maxLimitOfLoadout;
        public List<UpgradeDefinitionSO> cardUpgradeList = new();

        
        public event Action<List<UpgradeDefinitionSO>> OnLoadoutChanged;

        private void Start()
        {
            if (attachedPlayer == null)
            {
                throw new MissingReferenceException("ActiveLoadout does not have an reference to player");
            }
        }

        public void InitializeComponent(Player refPlayer, Player enemyRef, PlayerStats referencedStats)
        {
            attachedPlayer = refPlayer;
            enemyPlayer = enemyRef;
            maxLimitOfLoadout = referencedStats.cardSlots;

            //used for resetting any values for upgrades
            foreach (UpgradeDefinitionSO upgrade in cardUpgradeList)
            {
                upgrade.Initialize();
            }
        }

        public GameResult ApplyResultAlteringEffect(GameResult initialResult)
        {
            GameResult alteredResult = initialResult;
            foreach (UpgradeDefinitionSO upgrade in cardUpgradeList)
            {
                alteredResult = upgrade.ApplyResultAlteringEffect(initialResult, attachedPlayer, enemyPlayer);
            }
            
            return alteredResult;
        }

        /// <summary>
        /// defaulted inputData is null, pass any inputData that has the IAbilityInputData interface.
        /// </summary>
        public void HandleActivateSkill(UpgradeDefinitionSO upgrade, IAbilityInputData inputData = null)
        {
            if (cardUpgradeList.Contains(upgrade) && upgrade.isActivatable)
            {
                upgrade.ApplyActivatableEffect(attachedPlayer, enemyPlayer, inputData);
            }
        }
        
        public PlayerStats InitializeStatUpgrade(PlayerStats cardStats)
        {
            //assume this is turn 1, only used for player to self Initialize
            return ApplyStatUpgrade(cardStats, 1);
        }

        public PlayerStats ApplyStatUpgrade(PlayerStats cardStats, int currentTurnCount)
        {
            PlayerStats tempPlayerStats = new();
            PlayerStats tempEnemyStats = new();

            foreach (UpgradeDefinitionSO upgrade in cardUpgradeList)
            {
                upgrade.ApplyStatUpgrade(tempPlayerStats, tempEnemyStats, currentTurnCount);
            }

            cardStats = tempPlayerStats;

            return cardStats;
        }

        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("ActiveLoadout/Apply Passive Effect")]
        public void ApplyPassiveEffects(int currentTurnCount)
        {
            foreach (UpgradeDefinitionSO upgrade in cardUpgradeList)
            {
                upgrade.ApplyPassiveEffect(attachedPlayer, enemyPlayer, currentTurnCount);
            }
        }

        /// <summary>
        /// used in contextmenu as well
        /// </summary>
        [ContextMenu("ActiveLoadout/Apply OnWin Effect")]
        public void ApplyOnWinEffects()
        {
            foreach (UpgradeDefinitionSO upgrade in cardUpgradeList)
            {
                upgrade.OnWinRound(attachedPlayer, enemyPlayer);
            }
        }

        /// <summary>
        /// used in contextmenu as well
        /// </summary>
        [ContextMenu("ActiveLoadout/Apply OnLose Effect")]
        public void ApplyOnLoseEffect()
        {
            foreach (UpgradeDefinitionSO upgrade in cardUpgradeList)
            {
                upgrade.OnLoseRound(attachedPlayer, enemyPlayer);
            }
        }

        /// <summary>
        /// used in contextmenu as well.
        /// </summary>
        [ContextMenu("ActiveLoadout/Apply OnDraw Effect")]
        public void ApplyOnDrawEffect()
        {
            foreach (UpgradeDefinitionSO upgrade in cardUpgradeList)
            {
                upgrade.OnDrawRound(attachedPlayer, enemyPlayer);
            }
        }

        /// <summary>
        /// overloaded method for adding to loadout and requires an instance/reference to add.
        /// </summary>
        public void AddUpgradeToLoadout(UpgradeDefinitionSO upgrade)
        {
            cardUpgradeList.Add(upgrade);
            OnLoadoutChanged?.Invoke(cardUpgradeList);
        }

        /// <summary>
        /// overloaded method for adding to loadout and does not require an instance/reference to add, just the type.
        /// </summary>
        public void AddUpgradeToLoadout(UpgradeType upgradeType)
        {
            var createdUpgrade = UpgradeSOFactory.CreateUpgradeDefinitionSO(upgradeType);
            cardUpgradeList.Add(createdUpgrade);
            OnLoadoutChanged?.Invoke(cardUpgradeList);
        }

        public void RemoveUpgradeFromLoadout(UpgradeType upgradeType)
        {
            var removedUpgrade = cardUpgradeList.Find(upgrade => upgrade.upgradeType == upgradeType);
            cardUpgradeList.Remove(removedUpgrade);
            OnLoadoutChanged?.Invoke(cardUpgradeList);
        }
        
        #region Debug

        ///<summary>
        /// Used for Context Menu
        ///</summary>
        [ContextMenu("Debug/Debug Attached Player")]
        public void DebugAttachedPlayer()
        {
            Debug.Log($"{attachedPlayer.GetType()}");
        }
        
        ///<summary>
        /// Used for Context Menu
        ///</summary>
        [ContextMenu("Debug/Add DevilUpgrade to Player")]
        public void AddUpgradeToPlayer()
        {
            AddUpgradeToLoadout(UpgradeType.TheFool);
        }

        ///<summary>
        /// Used for Context Menu
        ///</summary>
        [ContextMenu("Debug/Remove DevilUpgrade to Player")]
        public void RemoveUpgradeFromPlayer()
        {
            RemoveUpgradeFromLoadout(UpgradeType.TheFool);
        }

        #endregion
    }
}