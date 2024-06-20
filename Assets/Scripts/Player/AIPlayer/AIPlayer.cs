using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : Player
{
    public AIDecision aiModuleConfig;

    protected override void Awake()
    {
        base.Awake();

    }

    public override GameChoice GetChoice()
    {
        if(aiModuleConfig == null)
        {
            throw new MissingComponentException("AI Module is not assigned");
        }

        //#TODO: Add a way to check if the decision is possible before making decision
        return aiModuleConfig.MakeDecision();
    }
}
