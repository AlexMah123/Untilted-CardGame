using System.Linq;
using UnityEngine;

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

        if (!queriedSO) throw new MissingReferenceException("Tried to get a reference to a non existing upgradeSO");


        UpgradeDefinitionSO newSO = Instantiate(queriedSO);

        return newSO;
    }
}
