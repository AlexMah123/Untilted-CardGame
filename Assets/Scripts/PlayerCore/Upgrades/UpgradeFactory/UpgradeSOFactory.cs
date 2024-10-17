using System.Linq;
using PlayerCore.Upgrades.Base;
using PlayerCore.Upgrades.Collection;
using UnityEngine;

namespace PlayerCore.Upgrades.UpgradeFactory
{
    public enum UpgradeType
    {
        None,
        Death,
        Justice,
        TheChariot,
        TheDevil,
        TheEmperor,
        TheEmpress,
        TheFool,
        TheHighPriestess,
        TheLovers,
        TheSun,
        TheWheelOfFortune,
        TheMoon
    }

    public class UpgradeSOFactory : MonoBehaviour
    {
        public static UpgradeSOFactory Instance;

        [Header("Upgrade List")]
        public UpgradeCollectionSO totalPossibleUpgrade;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public static UpgradeDefinitionSO CreateUpgradeDefinitionSO(UpgradeType upgradeEnum)
        {
            UpgradeDefinitionSO queriedSO = Instance.totalPossibleUpgrade.upgradeList.FirstOrDefault(x => x.upgradeType == upgradeEnum);

            if (!queriedSO) throw new MissingReferenceException($"Tried to get a reference to a non existing upgradeSO: {upgradeEnum.ToString()}");


            UpgradeDefinitionSO newSO = Instantiate(queriedSO);

            return newSO;
        }
    }
}