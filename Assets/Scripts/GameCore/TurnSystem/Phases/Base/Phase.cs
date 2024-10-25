using PlayerCore;
using PlayerCore.AIPlayer;

namespace GameCore.TurnSystem.Phases.Base
{
    public abstract class Phase
    {
        protected TurnSystemManager Controller;

        #region Ran on every phase

        public void OnStartPhase(TurnSystemManager turnController, Player player, AIPlayer aiPlayer)
        {
            Controller = turnController;

            OnStart(player, aiPlayer);
        }

        public void OnUpdatePhase(Player player, AIPlayer aiPlayer)
        {
            OnUpdate(player, aiPlayer);
        }

        public void OnEndPhase(Player player, AIPlayer aiPlayer)
        {
            OnEnd(player, aiPlayer);
        }

        #endregion


        //Overrides for inherited classes
        protected abstract void OnStart(Player player, AIPlayer aiPlayer);

        protected abstract void OnUpdate(Player player, AIPlayer aiPlayer);

        protected abstract void OnEnd(Player player, AIPlayer aiPlayer);
    }
}