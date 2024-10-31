using System;
using GameCore;
using PlayerCore.Upgrades.UpgradeFactory;
using UnityEngine;

namespace PlayerCore.Upgrades.Base
{
    public abstract class UpgradeDefinitionSO : ScriptableObject
    {
        [Header("User Interface Configs")] 
        public Sprite upgradeSprite;
        public string upgradeName;

        [Multiline] public string upgradeDescription;

        [Header("Gameplay Configs")] 
        public bool isInEffect = false;
        public UpgradeType upgradeType;

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

        //used if you need to apply an effect before win,lose,draw
        public virtual GameResult ApplyResultAlteringEffect(GameResult initialResult, Player attachedPlayer, Player enemyPlayer)
        {
            return initialResult;
        }
        
        #region CardUpgrade Effects
        //return nothing if there is no stat upgrade
        public abstract (PlayerStats playerstats, PlayerStats enemyStats) ApplyStatUpgrade(PlayerStats playerCardStats,
            PlayerStats enemyCardStats, int currentTurnCount);

        public virtual void ApplyPassiveEffect(Player attachedPlayer, Player enemyPlayer, int currentTurnCount)
        {
            //Debug.Log($"Applying Passive Effect of {this.GetType()}, {attachedPlayer}, {enemyPlayer}");
        }

        public virtual void ApplyActivatableEffect(Player attachedPlayer, Player enemyPlayer)
        {
            //Debug.Log($"Activating Effect of {this.GetType()}");
        }

        public virtual void OnWinRound(Player attachedPlayer, Player enemyPlayer)
        {
            //Debug.Log($"Applying OnWin Effect of {this.GetType()}");
        }

        public virtual void OnLoseRound(Player attachedPlayer, Player enemyPlayer)
        {
            //Debug.Log($"Applying OnLose Effect of {this.GetType()}");
        }

        public virtual void OnDrawRound(Player attachedPlayer, Player enemyPlayer)
        {
            //Debug.Log($"Applying OnDraw Effect of {this.GetType()}");
        }
        #endregion
    }
}