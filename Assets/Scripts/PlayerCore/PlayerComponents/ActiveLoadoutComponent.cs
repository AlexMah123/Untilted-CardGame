using System.Collections.Generic;
using GameCore.LoadoutSelection;
using PlayerCore.Upgrades.Base;
using PlayerCore.Upgrades.UpgradeFactory;
using UnityEngine;

namespace PlayerCore.PlayerComponents
{
    public class ActiveLoadoutComponent : MonoBehaviour
    {
        [Header("Runtime Value")]
        public int maxLimitOfLoadout;
        public List<UpgradeDefinitionSO> upgradeCardSlots = new();

        [HideInInspector]
        public Player attachedPlayer;

        public Player enemyPlayer;

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

        public void HandleActivateSkill(UpgradeDefinitionSO info)
        {
            if (upgradeCardSlots.Contains(info))
            {
                info.ApplyActivatableEffect(attachedPlayer, enemyPlayer);
            }
        }

        [ContextMenu("ActiveLoadout/Apply Passive Effect")]
        public void ApplyPassiveEffects()
        {
            foreach (UpgradeDefinitionSO upgrade in upgradeCardSlots)
            {
                upgrade.ApplyPassiveEffect(attachedPlayer, enemyPlayer);
            }
        }

        [ContextMenu("ActiveLoadout/Apply OnWin Effect")]
        public void ApplyOnWinEffects()
        {
            foreach (UpgradeDefinitionSO upgrade in upgradeCardSlots)
            {
                upgrade.OnWinRound(attachedPlayer, enemyPlayer);
            }
        }


        [ContextMenu("ActiveLoadout/Apply OnLose Effect")]
        public void ApplyOnLoseEffect()
        {
            foreach (UpgradeDefinitionSO upgrade in upgradeCardSlots)
            {
                upgrade.OnLoseRound(attachedPlayer, enemyPlayer);
            }
        }

        [ContextMenu("ActiveLoadout/Apply OnDraw Effect")]
        public void ApplyOnDrawEffect()
        {
            foreach (UpgradeDefinitionSO upgrade in upgradeCardSlots)
            {
                upgrade.OnDrawRound(attachedPlayer, enemyPlayer);
            }
        }

        public List<UpgradeDefinitionSO> FetchActiveLoadout()
        {
            return upgradeCardSlots;
        }

        public void AddUpgradeToLoadout(UpgradeDefinitionSO upgrade)
        {
            upgradeCardSlots.Add(upgrade);
        }

        public void AddUpgradeToLoadout(UpgradeType upgradeType)
        {
            var createdUpgrade = UpgradeSOFactory.CreateUpgradeDefinitionSO(upgradeType);
            upgradeCardSlots.Add(createdUpgrade);
        }


        #region Debug
        [ContextMenu("Debug/Debug Attached Player")]
        public void DebugAttachedPlayer()
        {
            Debug.Log($"{attachedPlayer.GetType()}");
        }
        #endregion

    }
}
