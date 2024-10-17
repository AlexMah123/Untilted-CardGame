using PlayerCore.Upgrades.UpgradeFactory;
using UnityEngine;

namespace PlayerCore.Upgrades.Base
{
    public abstract class UpgradeDefinitionSO : ScriptableObject
    {
        [Header("User Interface Configs")]
        public Sprite upgradeSprite;
        public string upgradeName;

        [Multiline]
        public string upgradeDescription;

        [Header("Gameplay Configs")]
        public bool isActivatable;
        public UpgradeType upgradeType;

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

        public virtual void ApplyAtStartOfGame(IPlayer attachedPlayer, IPlayer enemyPlayer)
        {
            Debug.Log($"Applying Stat Effect of {this.GetType()}, {attachedPlayer}, {enemyPlayer}");
        }
        
        public virtual void ApplyPassiveEffect(IPlayer attachedPlayer, IPlayer enemyPlayer)
        {
            Debug.Log($"Applying Passive Effect of {this.GetType()}, {attachedPlayer}, {enemyPlayer}");
        }

        public virtual void ApplyActivatableEffect(IPlayer attachedPlayer, IPlayer enemyPlayer)
        {
            Debug.Log($"Activating Effect of {this.GetType()}");
        }

        public virtual void OnWinRound(IPlayer attachedPlayer, IPlayer enemyPlayer)
        {
            Debug.Log($"Applying OnWin Effect of {this.GetType()}");

        }

        public virtual void OnLoseRound(IPlayer attachedPlayer, IPlayer enemyPlayer)
        {
            Debug.Log($"Applying OnLose Effect of {this.GetType()}");

        }

        public virtual void OnDrawRound(IPlayer attachedPlayer, IPlayer enemyPlayer)
        {
            Debug.Log($"Applying OnDraw Effect of {this.GetType()}");

        }
    }
}
