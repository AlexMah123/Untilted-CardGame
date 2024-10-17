using PlayerCore.Upgrades.Base;
using UnityEngine;

namespace PlayerCore.Upgrades.PrimaryUpgrades
{
    [CreateAssetMenu(fileName = "TheLovers", menuName = "Upgrades/UpgradeDefiniton/TheLovers")]
    public class TheLovers : UpgradeDefinitionSO
    {
        [SerializeField] private int selfHeal = 1;
        [SerializeField] private int enemyHeal = 1;

        public override void ApplyAtStartOfGame(IPlayer attachedPlayer, IPlayer enemyPlayer)
        {
            base.ApplyAtStartOfGame(attachedPlayer, enemyPlayer);
        }

        public override void ApplyPassiveEffect(IPlayer attachedPlayer, IPlayer enemyPlayer)
        {
            base.ApplyPassiveEffect(attachedPlayer, enemyPlayer);
        }

        public override void ApplyActivatableEffect(IPlayer attachedPlayer, IPlayer enemyPlayer)
        {
            base.ApplyActivatableEffect(attachedPlayer, enemyPlayer);
        }

        public override void OnWinRound(IPlayer attachedPlayer, IPlayer enemyPlayer)
        {
            base.OnWinRound(attachedPlayer, enemyPlayer);
        }

        public override void OnLoseRound(IPlayer attachedPlayer, IPlayer enemyPlayer)
        {
            base.OnLoseRound(attachedPlayer, enemyPlayer);
        }

        public override void OnDrawRound(IPlayer attachedPlayer, IPlayer enemyPlayer)
        {
            base.OnDrawRound(attachedPlayer, enemyPlayer);
            
            attachedPlayer.HealthComponent.IncreaseHealth(selfHeal);
            enemyPlayer.HealthComponent.IncreaseHealth(enemyHeal);
        }
    }
}
