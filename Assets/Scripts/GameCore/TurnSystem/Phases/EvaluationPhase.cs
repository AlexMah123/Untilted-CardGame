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

            //check OnWin/Lose/Draw effects, only calculate dmg
            //change turn to start of round
            
            //get the result based on the played choice, then evaluate them (broadcast all the neccesary events)
            var finalChoice = GameManager.GetFinalChoice();
            var roundResult = GameUtilsLibrary.GetGameResult(finalChoice.playerChoice, finalChoice.aiChoice);
            GameManager.EvaluateResults(roundResult);
            
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