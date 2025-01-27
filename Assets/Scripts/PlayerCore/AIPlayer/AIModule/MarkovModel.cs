using System.Collections.Generic;
using UnityEngine;
using PlayerCore.AIPlayer.AIModule.Base;
using PlayerCore.PlayerComponents;

namespace PlayerCore.AIPlayer.AIModule
{
    [CreateAssetMenu(menuName = "Enemy/Decision/MarkovModelSO")]
    public class MarkovModel : AIDecision
    {
        //transition matrix
        private Dictionary<GameChoice, Dictionary<GameChoice, int>> transitionMatrix =
            new()
            {
                {
                    GameChoice.Rock,
                    new Dictionary<GameChoice, int>
                        { { GameChoice.Rock, 0 }, { GameChoice.Paper, 0 }, { GameChoice.Scissor, 0 } }
                },
                {
                    GameChoice.Paper,
                    new Dictionary<GameChoice, int>
                        { { GameChoice.Rock, 0 }, { GameChoice.Paper, 0 }, { GameChoice.Scissor, 0 } }
                },
                {
                    GameChoice.Scissor,
                    new Dictionary<GameChoice, int>
                        { { GameChoice.Rock, 0 }, { GameChoice.Paper, 0 }, { GameChoice.Scissor, 0 } }
                },
            };

        //assumed last move by opponent
        private GameChoice opponentLastChoice = GameChoice.Paper;

        public override GameChoice MakeDecision()
        {
            //defaulted values
            GameChoice aiDecision = GameChoice.Rock;
            GameChoice predictedMoveByOpponent = GameChoice.Rock;

            //if its the first time playing, return a random move
            if (!transitionMatrix.ContainsKey(opponentLastChoice)) return RandomChoice();
            ;

            //if the accuracy fails, return random
            if (Random.value > aiAccuracy) return RandomChoice();

            //gets the dictionary of the last move.
            var opponentNextMoveProbabilityDict = transitionMatrix[opponentLastChoice];
            int opponentMostThrownMove = 0;

            //loop through the dictionary based on the last thrown move and find the highest probability
            foreach (var move in opponentNextMoveProbabilityDict)
            {
                if (move.Value > opponentMostThrownMove)
                {
                    predictedMoveByOpponent = move.Key;
                    opponentMostThrownMove = move.Value;
                }
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
            transitionMatrix = new Dictionary<GameChoice, Dictionary<GameChoice, int>>()
            {
                {
                    GameChoice.Rock,
                    new Dictionary<GameChoice, int>
                        { { GameChoice.Rock, 0 }, { GameChoice.Paper, 0 }, { GameChoice.Scissor, 0 } }
                },
                {
                    GameChoice.Paper,
                    new Dictionary<GameChoice, int>
                        { { GameChoice.Rock, 0 }, { GameChoice.Paper, 0 }, { GameChoice.Scissor, 0 } }
                },
                {
                    GameChoice.Scissor,
                    new Dictionary<GameChoice, int>
                        { { GameChoice.Rock, 0 }, { GameChoice.Paper, 0 }, { GameChoice.Scissor, 0 } }
                },
            };
        }

        public override void UpdateAIModule(GameChoice opponentChoice)
        {
            if (transitionMatrix.ContainsKey(opponentLastChoice) &&
                transitionMatrix[opponentLastChoice].ContainsKey(opponentChoice))
            {
                transitionMatrix[opponentLastChoice][opponentChoice]++;
            }

            opponentLastChoice = opponentChoice;
        }


        private GameChoice GetNextBestChoice()
        {
            // Get a list of choices sorted by the opponents most thrown move from their last choice
            List<GameChoice> sortedOpponentMostThrownMove =
                new List<GameChoice>(transitionMatrix[opponentLastChoice].Keys);
            sortedOpponentMostThrownMove.Sort((choice1, choice2) =>
                transitionMatrix[opponentLastChoice][choice2].CompareTo(transitionMatrix[opponentLastChoice][choice1]));

            // Return the next available choice
            foreach (var choice in sortedOpponentMostThrownMove)
            {
                GameChoice counterChoice = PlayerDecisionLibrary.GetCounterChoice(choice);
                if (choiceComponent.IsChoiceAvailable(counterChoice))
                {
                    return counterChoice;
                }
            }

            //Fallback to a random choice if no choices are available (this should never happen)
            return RandomChoice();
        }
    }
}