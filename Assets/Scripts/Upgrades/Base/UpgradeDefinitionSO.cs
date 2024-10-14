using UnityEngine;

using PlayerCore;
using Upgrades.UpgradeFactory;

namespace Upgrades.Base
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

        public virtual void ApplyPassiveEffect(IPlayer player)
        {

        }

        public virtual void ApplyActivatableEffect(IPlayer player)
        {
            Debug.Log($"Activating Effect of {this.GetType()}");
        }

        public virtual void OnWinRound(IPlayer player)
        {
            Debug.Log($"Applying OnWin Effect of {this.GetType()}");

        }

        public virtual void OnLoseRound(IPlayer player)
        {
            Debug.Log($"Applying OnLose Effect of {this.GetType()}");

        }

        public virtual void OnDrawRound(IPlayer player)
        {
            Debug.Log($"Applying OnDraw Effect of {this.GetType()}");

        }
    }
}
