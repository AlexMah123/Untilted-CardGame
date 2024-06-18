using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : Turn
{
    protected override void OnStart(IPlayer currentPlayer)
    {
        Debug.Log("Start Player Turn");
    }

    protected override void OnUpdate(IPlayer currentPlayer)
    {
        Debug.Log("Update Player Turn");
    }

    protected override void OnEnd(IPlayer currentPlayer)
    {
        Debug.Log("End Player Turn");
    }
}
