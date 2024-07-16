using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthStatUI : MonoBehaviour
{
    public Player attachedPlayer;
    [SerializeField] TextMeshProUGUI healthText;    
    

    private void Start()
    {
        if(attachedPlayer != null)
        {
            attachedPlayer.HealthStatComponent.OnHealthModifiedEvent += HandleOnHealthModified;
        }
        else
        {
            throw new System.AccessViolationException("Attached Player is null");
        }

        //initial call to set values
        HandleOnHealthModified(attachedPlayer.HealthStatComponent.healthAmount);
    }

    private void HandleOnHealthModified(int newHealth)
    {
        healthText.text = newHealth.ToString();
    }
}
