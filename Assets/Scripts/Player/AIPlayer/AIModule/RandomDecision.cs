using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DecisionSO", menuName = "Enemy/Decision/RandomDecisionSO")]
public class RandomDecision : AIDecision
{
    public override GameChoice MakeDecision()
    {
        return RandomChoice();
    }
}
