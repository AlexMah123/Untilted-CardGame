using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayer : Player
{
    protected override void Awake()
    {
        base.Awake();

        //StatComponent.SealChoice(GameChoice.ROCK);
    }

    public override void GetChoice()
    {

    }
}
