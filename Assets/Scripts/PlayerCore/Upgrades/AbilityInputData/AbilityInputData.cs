using PlayerCore.PlayerComponents;
using PlayerCore.Upgrades.UpgradeFactory;
using UnityEngine;

namespace PlayerCore.Upgrades.AbilityInputData
{
    public interface IAbilityInputData { };

    public class ChoiceSealInputData : IAbilityInputData
    {
        public GameChoice choiceToSeal;

        public ChoiceSealInputData(GameChoice choiceToSeal)
        {
            this.choiceToSeal = choiceToSeal;
            Debug.Log(choiceToSeal);
        }
    }

    public class TargetUpgradeInputData : IAbilityInputData
    {
        public UpgradeType targetUpgrade;

        public TargetUpgradeInputData(UpgradeType targetUpgrade)
        {
            this.targetUpgrade = targetUpgrade;
        }
    }
}