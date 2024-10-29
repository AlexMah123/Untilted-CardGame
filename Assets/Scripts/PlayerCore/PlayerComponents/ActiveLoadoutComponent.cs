using System;
using System.Collections.Generic;
using GameCore;
using GameCore.LoadoutSelection;
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

        public void HandleActivateSkill(UpgradeDefinitionSO info)
        {
            if (cardUpgradeList.Contains(info))
            {
                info.ApplyActivatableEffect(attachedPlayer, enemyPlayer);
            }
        }

        public PlayerStats ApplyStatUpgrade(PlayerStats cardStats)
        {
            PlayerStats tempPlayerStats = new();
            PlayerStats tempEnemyStats = new();

            foreach (UpgradeDefinitionSO upgrade in cardUpgradeList)
            {
                upgrade.ApplyStatUpgrade(tempPlayerStats, tempEnemyStats);
            }

            cardStats = tempPlayerStats;

            return cardStats;
        }

        [ContextMenu("ActiveLoadout/Apply Passive Effect")]
        public void ApplyPassiveEffects()
        {
            foreach (UpgradeDefinitionSO upgrade in cardUpgradeList)
            {
                upgrade.ApplyPassiveEffect(attachedPlayer, enemyPlayer);
            }
        }

        [ContextMenu("ActiveLoadout/Apply OnWin Effect")]
        public void ApplyOnWinEffects()
        {
            foreach (UpgradeDefinitionSO upgrade in cardUpgradeList)
            {
                upgrade.OnWinRound(attachedPlayer, enemyPlayer);
            }
        }


        [ContextMenu("ActiveLoadout/Apply OnLose Effect")]
        public void ApplyOnLoseEffect()
        {
            foreach (UpgradeDefinitionSO upgrade in cardUpgradeList)
            {
                upgrade.OnLoseRound(attachedPlayer, enemyPlayer);
            }
        }

        [ContextMenu("ActiveLoadout/Apply OnDraw Effect")]
        public void ApplyOnDrawEffect()
        {
            foreach (UpgradeDefinitionSO upgrade in cardUpgradeList)
            {
                upgrade.OnDrawRound(attachedPlayer, enemyPlayer);
            }
        }

        public void AddUpgradeToLoadout(UpgradeDefinitionSO upgrade)
        {
            cardUpgradeList.Add(upgrade);
            OnLoadoutChanged?.Invoke(cardUpgradeList);
        }

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

        [ContextMenu("Debug/Debug Attached Player")]
        public void DebugAttachedPlayer()
        {
            Debug.Log($"{attachedPlayer.GetType()}");
        }

        [ContextMenu("Debug/Add DevilUpgrade to Player")]
        public void AddUpgradeToPlayer()
        {
            AddUpgradeToLoadout(UpgradeType.TheDevil);
        }

        [ContextMenu("Debug/Remove DevilUpgrade to Player")]
        public void RemoveUpgradeFromPlayer()
        {
            RemoveUpgradeFromLoadout(UpgradeType.TheDevil);
        }

        #endregion
    }
}