using System;
using GameCore.TurnSystem.Phases.Base;
using PlayerCore;
using PlayerCore.AIPlayer;
using UnityEngine;

namespace GameCore.TurnSystem.Phases
{
    public class StartOfRound : Phase
    {
        public event Action<int> TurnCountChanged;

        protected override void OnStart(Player player, AIPlayer aiPlayer)
        {
            Controller.turnCount++;
            TurnCountChanged?.Invoke(Controller.turnCount);
            
            //check passive
            player.ActiveLoadoutComponent.ApplyPassiveEffects(Controller.turnCount);
            aiPlayer.ActiveLoadoutComponent.ApplyPassiveEffects(Controller.turnCount);
            
            //update the current components if there are stat effects
            player.UpdateCurrentStats(Controller.turnCount);
            aiPlayer.UpdateCurrentStats(Controller.turnCount);
            
            
            Controller.ChangePhase(Controller.PlayerPhase);
        }

        protected override void OnUpdate(Player player, AIPlayer aiPlayer)
        {
        }

        protected override void OnEnd(Player player, AIPlayer aiPlayer)
        {
        }
    }
}