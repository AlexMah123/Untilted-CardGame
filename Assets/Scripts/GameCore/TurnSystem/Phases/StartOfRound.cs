using GameCore.TurnSystem.Phases.Base;
using PlayerCore;
using PlayerCore.AIPlayer;
using UnityEngine;

namespace GameCore.TurnSystem.Phases
{
    public class StartOfRound : Phase
    {
        protected override void OnStart(Player player, AIPlayer aiPlayer)
        {
            Debug.Log("Currently in StartOfRound");
            Controller.turnCount++;

            //load/update the current components
            player.UpdateCurrentStats();
            aiPlayer.UpdateCurrentStats();
            
            //check passive/stat effects below here.

            
            
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