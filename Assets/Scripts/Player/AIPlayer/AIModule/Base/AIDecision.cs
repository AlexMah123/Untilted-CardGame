using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIDecision : ScriptableObject
{
    [TextArea]
    public string Description;

    [Range(0, 1)]
    public float aiAccuracy = 1.0f;

    public abstract GameChoice MakeDecision();

    //override only whenever neccesary
    public virtual void UpdateAIModule(GameChoice opponentChoice)
    {

    }

    protected GameChoice RandomChoice()
    {
        return AIDecisionLibrary.GetRandomEnum<GameChoice>();
    }
}
