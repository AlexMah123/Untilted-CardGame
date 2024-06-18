using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turn
{
    protected TurnSystemManager controller;
    public event Action<IPlayer> OnPlayerStartTurn;

    #region Ran on every phase
    public void OnStartTurn(TurnSystemManager turnController, IPlayer currentPlayer)
    {
        controller = turnController;

        OnStart(currentPlayer);
        OnPlayerStartTurn?.Invoke(currentPlayer);
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
