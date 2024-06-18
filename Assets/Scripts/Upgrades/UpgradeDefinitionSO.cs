using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrades/UpgradeDefiniton")]
public class UpgradeDefinitionSO : ScriptableObject
{
    public Image upgradeImage;
    public string upgradeName;
    
    [Multiline]
    public string upgradeDefinition;
}
