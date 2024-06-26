using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : Player
{
    public AIDecision aiModuleConfig;

    protected override void Awake()
    {
        base.Awake();

        aiModuleConfig.InitializeAIConfig(ChoiceComponent);
        //ChoiceComponent.SealChoice(GameChoice.ROCK);
        //ChoiceComponent.SealChoice(GameChoice.PAPER);

    }

    public override GameChoice GetChoice()
    {
        if(aiModuleConfig == null)
        {
            throw new MissingComponentException("AI Module is not assigned");
        }

        return aiModuleConfig.MakeDecision();
    }
}
