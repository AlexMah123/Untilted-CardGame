using UnityEngine;

namespace LoadoutSelection.LoadoutCardObj
{
    public class LoadoutCardFactory : MonoBehaviour
    {
        public GameObject loadoutCardPrefab;

        public GameObject CreateUpgradeCard(FLoadoutCardCreation creation)
        {
            GameObject upgradeCardObj = Instantiate(loadoutCardPrefab, creation.parent);

            return upgradeCardObj;
        }
    }
}