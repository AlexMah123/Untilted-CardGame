using System;
using PlayerCore;

namespace GameCore.TurnSystem.Turns.Base
{
    public abstract class Turn
    {
        protected TurnSystemManager controller;
        public event Action<Player> OnPlayerTurnStart;

        #region Ran on every phase
        public void OnStartTurn(TurnSystemManager turnController, Player currentPlayer)
        {
            controller = turnController;

            OnStart(currentPlayer);

            //broadcast event, primarily binded to PlayerHandUIManager
            OnPlayerTurnStart?.Invoke(currentPlayer);
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
}
