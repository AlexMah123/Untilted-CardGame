using System.Collections.Generic;
using UnityEngine;
using PlayerCore.AIPlayer.AIModule.Base;
using PlayerCore.PlayerComponents;

namespace PlayerCore.AIPlayer.AIModule
{
    [CreateAssetMenu(menuName = "Enemy/Decision/BayesianModelSO")]
    public class BayesianModel : AIDecision
    {
        private Dictionary<GameChoice, int> opponentMoveCount = new Dictionary<GameChoice, int>
        {
            { GameChoice.Rock, 0 },
            { GameChoice.Paper, 0 },
            { GameChoice.Scissor, 0 }
        };

        public override GameChoice MakeDecision()
        {
            //defaulted values
            GameChoice aiDecision = GameChoice.Rock;

            //sum the total thrown moves by the opponent
            int totalMoves = opponentMoveCount[GameChoice.Rock] + opponentMoveCount[GameChoice.Paper] + opponentMoveCount[GameChoice.Scissor];

            //if its the first time playing, return a random move
            if (totalMoves == 0) return RandomChoice();

            //if the accuracy fails, return random
            if (Random.value > aiAccuracy) return RandomChoice();

            //calculate the individual probability against the total amount of moves the player has thrown.
            float rockProbability = (float)opponentMoveCount[GameChoice.Rock] / totalMoves;
            float paperProbability = (float)opponentMoveCount[GameChoice.Paper] / totalMoves;
            float scissorsProbability = (float)opponentMoveCount[GameChoice.Scissor] / totalMoves;

            //defaulted
            GameChoice predictedMoveByOpponent = GameChoice.Rock;

            if (rockProbability >= paperProbability && rockProbability >= scissorsProbability)
            {
                predictedMoveByOpponent = GameChoice.Rock;
            }
            else if (paperProbability >= rockProbability && paperProbability >= scissorsProbability)
            {
                predictedMoveByOpponent = GameChoice.Paper;
            }
            else
            {
                predictedMoveByOpponent = GameChoice.Scissor;
            }

            aiDecision = PlayerDecisionLibrary.GetCounterChoice(predictedMoveByOpponent);
            if (choiceComponent.IsChoiceAvailable(aiDecision))
            {
                return aiDecision;
            }
            else
            {
                return GetNextBestChoice();
            }

        }

        public override void ResetAIConfig()
        {
            opponentMoveCount = new Dictionary<GameChoice, int>
            {
                { GameChoice.Rock, 0 },
                { GameChoice.Paper, 0 },
                { GameChoice.Scissor, 0 }
            };
        }

        public override void UpdateAIModule(GameChoice opponentChoice)
        {
            if (opponentMoveCount.ContainsKey(opponentChoice))
            {
                opponentMoveCount[opponentChoice]++;
            }
        }

        private GameChoice GetNextBestChoice()
        {
            // Get a list of choices sorted by the opponents most thrown move
            List<GameChoice> sortedOpponentMostThrownMove = new List<GameChoice>(opponentMoveCount.Keys);
            sortedOpponentMostThrownMove.Sort((choice1, choice2) => opponentMoveCount[choice2].CompareTo(opponentMoveCount[choice1]));

            // Return the next available choice
            foreach (GameChoice choice in sortedOpponentMostThrownMove)
            {
                GameChoice counterChoice = PlayerDecisionLibrary.GetCounterChoice(choice);
                if (choiceComponent.IsChoiceAvailable(choice))
                {
                    return counterChoice;
                }
            }

            //Fallback to a random choice if no choices are available (this should never happen)
            return RandomChoice();
        }
    }
}
