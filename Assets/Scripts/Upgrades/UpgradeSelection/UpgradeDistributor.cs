using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum UPGRADE_TYPE
{
    NONE,
    DEATH,
    JUSTICE,
    THE_CHARIOT,
    THE_DEVIL,
    THE_EMPEROR,
    THE_EMPRESS,
    THE_FOOL,
    THE_HIGH_PRIESTESS,
    THE_LOVERS,
    THE_SUN,
    THE_WHEEL_OF_FORTUNE,
}

public class UpgradeDistributor : MonoBehaviour
{
    public static UpgradeDistributor Instance;

    [Header("Upgrade List")]
    public List<UpgradeDefinitionSO> totalPossibleUpgrade = new();

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public static UpgradeDefinitionSO FetchUpgradeDefinitionSO(UPGRADE_TYPE upgradeEnum)
    {
        UpgradeDefinitionSO queriedSO = Instance.totalPossibleUpgrade.FirstOrDefault(x => x.upgradeType == upgradeEnum);

        if (!queriedSO) throw new MissingReferenceException("Tried to get a reference to a non existing upgradeSO");


        UpgradeDefinitionSO newSO = Instantiate(queriedSO);

        return newSO;
    }
}
