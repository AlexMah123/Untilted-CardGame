using System;
using GameCore;
using PlayerCore.Upgrades.AbilityInputData;
using PlayerCore.Upgrades.UpgradeFactory;
using UnityEngine;

namespace PlayerCore.Upgrades.Base
{
    public enum AbilityConfirmationType
    {
        None,
        ChoiceSeal,
        TargetUpgrade,
    }
    
    public abstract class UpgradeDefinitionSO : ScriptableObject
    {
        [Header("User Interface Configs")] 
        public Sprite upgradeSprite;
        public string upgradeName;

        [Multiline] public string upgradeDescription;

        [Header("Gameplay Configs")] 
        public bool isActivatable = false;
        public UpgradeType upgradeType;
        public AbilityConfirmationType abilityConfirmationType = AbilityConfirmationType.None;

        #region Overrides
        public override bool Equals(object obj)
        {
            if (obj is UpgradeDefinitionSO other)
            {
                return upgradeType == other.upgradeType;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return upgradeType.GetHashCode();
        }

        public virtual void Initialize()
        {
            
        }
        #endregion

        /// <summary>
        /// used if you need to apply an effect before win,lose,draw
        /// Also used for any round changing effects
        /// </summary>
        public virtual GameResult ApplyResultAlteringEffect(GameResult initialResult, Player attachedPlayer, Player enemyPlayer)
        {
            return initialResult;
        }
        
        #region CardUpgrade Effects
        /// <summary>
        /// mandatory override, return nothing if there is no stat upgrade
        /// </summary>
        public abstract (PlayerStats playerstats, PlayerStats enemyStats) ApplyStatUpgrade(PlayerStats playerCardStats,
            PlayerStats enemyCardStats, int currentTurnCount);

        
        /// <summary>
        /// override if necessary, used to apply any passive effects that are not one time used/activatable, stat effects or Conditional (WinLoseDraw)
        /// </summary>
        public virtual void ApplyPassiveEffect(Player attachedPlayer, Player enemyPlayer, int currentTurnCount)
        {
            //Debug.Log($"Applying Passive Effect of {this.GetType()}, {attachedPlayer}, {enemyPlayer}");
        }

        /// <summary>
        /// override if necessary, used for any activatable effect
        ///</summary>
        public virtual void ApplyActivatableEffect(Player attachedPlayer, Player enemyPlayer,
            IAbilityInputData inputData)
        {
            //Debug.Log($"Activating Effect of {this.GetType()}");
        }

        /// <summary>
        /// override if necessary, used for any OnWin effects
        ///</summary>
        public virtual void OnWinRound(Player attachedPlayer, Player enemyPlayer)
        {
            //Debug.Log($"Applying OnWin Effect of {this.GetType()}");
        }

        /// <summary>
        /// override if necessary, used for any OnLose effects
        ///</summary>
        public virtual void OnLoseRound(Player attachedPlayer, Player enemyPlayer)
        {
            //Debug.Log($"Applying OnLose Effect of {this.GetType()}");
        }

        /// <summary>
        /// override if necessary, used for any OnDraw effects
        ///</summary>
        public virtual void OnDrawRound(Player attachedPlayer, Player enemyPlayer)
        {
            //Debug.Log($"Applying OnDraw Effect of {this.GetType()}");
        }
        #endregion
    }
}