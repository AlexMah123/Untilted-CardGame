using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Decision/BayesianModelSO")]
public class BayesianModel : AIDecision
{
    private Dictionary<GameChoice, int> opponentMoveCount = new Dictionary<GameChoice, int>
    {
        { GameChoice.ROCK, 0 },
        { GameChoice.PAPER, 0 },
        { GameChoice.SCISSOR, 0 }
    };

    public override GameChoice MakeDecision()
    {
        //defaulted values
        GameChoice aiDecision = GameChoice.ROCK;

        //sum the total thrown moves by the opponent
        int totalMoves = opponentMoveCount[GameChoice.ROCK] + opponentMoveCount[GameChoice.PAPER] + opponentMoveCount[GameChoice.SCISSOR];

        //if its the first time playing, return a random move
        if (totalMoves == 0) return RandomChoice();

        //if the accuracy fails, return random
        if (Random.value > aiAccuracy) return RandomChoice();

        //calculate the individual probability against the total amount of moves the player has thrown.
        float rockProbability = (float)opponentMoveCount[GameChoice.ROCK] / totalMoves;
        float paperProbability = (float)opponentMoveCount[GameChoice.PAPER] / totalMoves;
        float scissorsProbability = (float)opponentMoveCount[GameChoice.SCISSOR] / totalMoves;

        //defaulted
        GameChoice predictedMoveByOpponent = GameChoice.ROCK;

        if (rockProbability >= paperProbability && rockProbability >= scissorsProbability)
        {
            predictedMoveByOpponent = GameChoice.ROCK;
        }
        else if(paperProbability >= rockProbability && paperProbability >= scissorsProbability)
        {
            predictedMoveByOpponent = GameChoice.PAPER;
        }
        else
        {
            predictedMoveByOpponent = GameChoice.SCISSOR;
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
        foreach (var choice in sortedOpponentMostThrownMove)
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
