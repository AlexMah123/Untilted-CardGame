using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Decision/MarkovModelSO")]
public class MarkovModel : AIDecision
{
    //transition matrix
    private Dictionary<GameChoice, Dictionary<GameChoice, int>> transitionMatrix = new Dictionary<GameChoice, Dictionary<GameChoice, int>>()
    {
        { GameChoice.ROCK, new Dictionary<GameChoice, int>{ {GameChoice.ROCK, 0}, {GameChoice.PAPER, 0}, {GameChoice.SCISSOR, 0} } },
        { GameChoice.PAPER, new Dictionary<GameChoice, int>{ {GameChoice.ROCK, 0}, {GameChoice.PAPER, 0}, {GameChoice.SCISSOR, 0} } },
        { GameChoice.SCISSOR, new Dictionary<GameChoice, int>{ {GameChoice.ROCK, 0}, {GameChoice.PAPER, 0}, {GameChoice.SCISSOR, 0} } },
    };

    //assumed last move by opponent
    private GameChoice opponentLastChoice = GameChoice.ROCK;

    public override GameChoice MakeDecision()
    {
        //defaulted values
        GameChoice aiDecision = GameChoice.ROCK;
        GameChoice predictedMoveByOpponent = GameChoice.ROCK;

        //if its the first time playing, return a random move
        if (!transitionMatrix.ContainsKey(opponentLastChoice)) return RandomChoice(); ;

        //if the accuracy fails, return random
        if (Random.value > aiAccuracy) return RandomChoice();

        //gets the dictionary of the last move.
        var opponentNextMoveProbabilityDict = transitionMatrix[opponentLastChoice];
        int opponentMostThrownMove = 0;

        //loop through the dictionary based on the last thrown move and find the highest probability
        foreach (var move in opponentNextMoveProbabilityDict)
        {
            if(move.Value > opponentMostThrownMove)
            {
                predictedMoveByOpponent = move.Key;
                opponentMostThrownMove = move.Value;
            }
        }

        aiDecision = AIDecisionLibrary.GetCounterChoice(predictedMoveByOpponent);

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
        if(transitionMatrix.ContainsKey(opponentLastChoice) && transitionMatrix[opponentLastChoice].ContainsKey(opponentChoice))
        {
            transitionMatrix[opponentLastChoice][opponentChoice]++;
        }
        opponentLastChoice = opponentChoice;
    }


    private GameChoice GetNextBestChoice()
    {
        // Get a list of choices sorted by the opponents most thrown move from their last choice
        List<GameChoice> sortedOpponentMostThrownMove = new List<GameChoice>(transitionMatrix[opponentLastChoice].Keys);
        sortedOpponentMostThrownMove.Sort((choice1, choice2) => transitionMatrix[opponentLastChoice][choice2].CompareTo(transitionMatrix[opponentLastChoice][choice1]));

        // Return the next available choice
        foreach (var choice in sortedOpponentMostThrownMove)
        {
            GameChoice counterChoice = AIDecisionLibrary.GetCounterChoice(choice);
            if (choiceComponent.IsChoiceAvailable(counterChoice))
            {
                return counterChoice;
            }
        }

        //Fallback to a random choice if no choices are available (this should never happen)
        return RandomChoice();
    }
}
