using System;
using System.Linq;
using PlayerCore.Upgrades.Base;
using PlayerCore.Upgrades.UpgradeFactory;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.AbilityInput.Factory
{
    public class AbilityInputButtonFactory : MonoBehaviour
    {
        public GameObject CreateAbilityInputButton(GameObject upgradePrefab, UpgradeDefinitionSO upgradeSO, Transform parentTransform, Action<UpgradeType> onClickEvent)
        {
            GameObject upgradeObject = Instantiate(upgradePrefab, parentTransform.transform);
                
            Image image = upgradeObject.GetComponentsInChildren<Image>(true).FirstOrDefault(x => x.gameObject != upgradeObject.gameObject);
            if (image != null)
            {
                image.sprite = upgradeSO.upgradeSprite;
            }
            
            Button upgradeButton = upgradeObject.GetComponentsInChildren<Button>(true).FirstOrDefault(x => x.gameObject != upgradeObject.gameObject);
            if (upgradeButton != null)
            {
                upgradeButton.onClick.AddListener(()=> onClickEvent(upgradeSO.upgradeType));
            }
            
            return upgradeObject;
        }
    }
}