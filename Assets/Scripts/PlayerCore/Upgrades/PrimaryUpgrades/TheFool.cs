using PlayerCore.Upgrades.Base;
using UnityEngine;

namespace PlayerCore.Upgrades.PrimaryUpgrades
{
    [CreateAssetMenu(fileName = "TheFool", menuName = "Upgrades/UpgradeDefiniton/TheFool")]
    public class TheFool : UpgradeDefinitionSO
    {
        [SerializeField] private int healthIncrease = 2;


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
        }
    }
}
