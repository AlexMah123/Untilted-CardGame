using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Turn
{
    protected TurnSystemManager controller;
    public event Action<Player> OnPlayerStartTurnEvent;

    #region Ran on every phase
    public void OnStartTurn(TurnSystemManager turnController, Player currentPlayer)
    {
        controller = turnController;

        OnStart(currentPlayer);

        //broadcast event, primarily binded to PlayerHandUIManager
        OnPlayerStartTurnEvent?.Invoke(currentPlayer);
    }

    public void OnUpdateTurn(Player currentPlayer)
    {
        OnUpdate(currentPlayer);
    }

    public void OnEndTurn(Player currentPlayer)
    {
        OnEnd(currentPlayer);
    }
    #endregion


    //Overrides for inherited classes
    protected abstract void OnStart(Player currentPlayer);

    protected abstract void OnUpdate(Player currentPlayer);

    protected abstract void OnEnd(Player currentPlayer);
}
