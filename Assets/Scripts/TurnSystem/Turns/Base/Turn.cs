using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turn
{
    #region Ran on every phase
    public void OnStartTurn(IPlayer currentPlayer)
    {
        OnStart(currentPlayer);
    }

    public void OnUpdateTurn(IPlayer currentPlayer)
    {
        OnUpdate(currentPlayer);
    }

    public void OnEndTurn(IPlayer currentPlayer)
    {
        OnEnd(currentPlayer);
    }
    #endregion


    //Overrides for inherited classes
    protected abstract void OnStart(IPlayer currentPlayer);

    protected abstract void OnUpdate(IPlayer currentPlayer);

    protected abstract void OnEnd(IPlayer currentPlayer);
}
