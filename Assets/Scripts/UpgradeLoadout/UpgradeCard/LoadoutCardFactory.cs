using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LoadoutCardCreationInfo
{
    public LoadoutCardCreationInfo(Transform spawnParent)
    {
        parent = spawnParent;
    }

    public Transform parent;
}

public class LoadoutCardFactory : MonoBehaviour
{
    public GameObject loadoutCardPrefab;

    public GameObject CreateUpgradeCard(LoadoutCardCreationInfo creationInfo)
    {
        GameObject upgradeCardGO = Instantiate(loadoutCardPrefab, creationInfo.parent);

        return upgradeCardGO;
    }
}
