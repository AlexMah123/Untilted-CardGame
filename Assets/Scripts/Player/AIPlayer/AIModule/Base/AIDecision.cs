using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIDecision : ScriptableObject
{
    [TextArea]
    public string Description;

    [Range(0, 1)]
    public float aiAccuracy = 1.0f;

    //injected references
    protected ChoiceComponent choiceComponent;

    public void InitializeAIConfig(ChoiceComponent component)
    {
        choiceComponent = component;
    }

    public abstract GameChoice MakeDecision();

    //override only whenever neccesary
    public virtual void UpdateAIModule(GameChoice opponentChoice)
    {

    }

    protected GameChoice RandomChoice()
    {
        return PlayerDecisionLibrary.GetRandomGameChoice();
    }
}
