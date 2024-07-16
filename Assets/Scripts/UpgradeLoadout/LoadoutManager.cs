using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutManager : MonoBehaviour
{
    public static LoadoutManager Instance;

    public List<UpgradeDefinitionSO> totalUpgradesUnlocked = new();

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

    public void AddSelectedUpgradeToActive()
    {
        //#TODO: need the cards be added to active
        //#DEBUG
        Debug.Log($"Added card to active");
    }
}
