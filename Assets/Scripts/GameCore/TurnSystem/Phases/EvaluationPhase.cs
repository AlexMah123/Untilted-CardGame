using System;
using GameCore.TurnSystem.Phases.Base;
using PlayerCore;
using PlayerCore.AIPlayer;
using PlayerCore.PlayerComponents;
using UnityEngine;

namespace GameCore.TurnSystem.Phases
{
    public class EvaluationPhase : Phase
    {
        GameManager GameManager => GameManager.Instance;

        public event Action OnClearCardInHand;

        protected override void OnStart(Player player, AIPlayer aiPlayer)
        {
            Debug.Log("Currently in EvaluationPhase");

            //get the result based on the played choice, then evaluate them.
            //GameResult is relative to the player (eg: player win lose or draw)
            var finalChoice = GameManager.GetFinalChoice();
            var roundResult = GameUtilsLibrary.GetPlayerGameResult(finalChoice.playerChoice, finalChoice.aiChoice);
            
            //check OnWin/Lose/Draw effects, returns resultsChanged if results was actually changed
            var hasResultChanged = GameManager.EvaluateResults(ref roundResult);
            
            //call the finalized results which is damage. 
            GameManager.FinalizeResults(roundResult);
            
            //end the turn
            Controller.ChangePhase(Controller.StartOfRound);
        }

        protected override void OnUpdate(Player player, AIPlayer aiPlayer)
        {
            
        }

        protected override void OnEnd(Player player, AIPlayer aiPlayer)
        {
            //After results, reset the turns
            //broadcast event, primarily binded to PlayerHandUIManager
            OnClearCardInHand?.Invoke();

            //#TODO: add some sort of delay between rounds for animation?
        }
    }
}