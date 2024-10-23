using System;
using GameCore.TurnSystem.Phases.Base;
using PlayerCore;
using PlayerCore.AIPlayer;
using UnityEngine;

namespace GameCore.TurnSystem.Phases
{
    public class PlayerPhase : Phase
    {
        public event Action<Player> OnPlayerPhaseStart;

        
        protected override void OnStart(Player player, AIPlayer aiPlayer)
        {
            Debug.Log("Currently in PlayerPhase");
            
            //broadcast event, primarily binded to PlayerHandUIManager
            OnPlayerPhaseStart?.Invoke(player);
        }

        protected override void OnUpdate(Player player, AIPlayer aiPlayer)
        {
            
        }

        protected override void OnEnd(Player player, AIPlayer aiPlayer)
        {
            
        }
    }
}
