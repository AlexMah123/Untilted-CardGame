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

            //decrement all seal effects
            DecrementPlayersSealedDuration(player, aiPlayer);
            
            //Clear players hands
            //broadcast event, primarily binded to PlayerHandUIManager
            OnClearCardInHand?.Invoke();
            
            //Evaluate the final result including applying damage and effects.
            EvaluateAndApplyFinalResult();
            
            //#TODO: add some sort of delay between rounds for animation?
            //end the turn
            Controller.ChangePhase(Controller.StartOfRound);
        }

        protected override void OnUpdate(Player player, AIPlayer aiPlayer)
        {
            
        }

        protected override void OnEnd(Player player, AIPlayer aiPlayer)
        {
            
        }
        
        
        #region Internal

        private void DecrementPlayersSealedDuration(Player player, AIPlayer aiPlayer)
        {
            //reset the choice duration (it decrements any sealed choice duration)
            player.ChoiceComponent.ResetChoicesAvailable();
            aiPlayer.ChoiceComponent.ResetChoicesAvailable();
        }
        
        private void EvaluateAndApplyFinalResult()
        {
            //get the result based on the played choice, then evaluate them.
            //GameResult is relative to the player (eg: player win lose or draw)
            var finalChoice = GameManager.GetFinalChoice();
            var roundResult = GameUtilsLibrary.GetPlayerGameResult(finalChoice.playerChoice, finalChoice.aiChoice);
            
            //check OnWin/Lose/Draw effects, returns resultsChanged if results was actually changed
            var hasResultChanged = GameManager.EvaluateResults(ref roundResult);
            
            //#TODO: if result has changed, maybe do an animation?
            
            ApplyFinalResult(roundResult);
        }

        private void ApplyFinalResult(GameResult finalResult)
        {
            //Apply all the effects
            GameManager.ApplyRoundEffects(finalResult);
            
            //call the finalized result that is damage. 
            GameManager.ApplyDamage(finalResult);
        }
        #endregion
    }
}