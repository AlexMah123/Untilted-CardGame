using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : Player
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override GameChoice GetChoice()
    {
        return GameChoice.PAPER;
    }
}
