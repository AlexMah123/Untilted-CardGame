using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Decision/SpecificChoiceModel")]
public class SpecificChoiceModel : AIDecision
{
    [SerializeField] GameChoice choice;

    public override GameChoice MakeDecision()
    {
        return choice;
    }

    public override void ResetAIConfig()
    {
        
    }
}
