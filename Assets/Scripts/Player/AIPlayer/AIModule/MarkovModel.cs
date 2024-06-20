using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DecisionSO", menuName = "Enemy/Decision/MarkovModelSO")]
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
        GameChoice predictedMove = GameChoice.ROCK;

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
                predictedMove = move.Key;
                opponentMostThrownMove = move.Value;
            }
        }

        switch(predictedMove)
        {
            case GameChoice.ROCK:
                aiDecision = GameChoice.PAPER;
                break;

            case GameChoice.PAPER:
                aiDecision = GameChoice.SCISSOR;
                break;

            case GameChoice.SCISSOR:
                aiDecision = GameChoice.ROCK;
                break;
        }


        return aiDecision;
    }

    public override void UpdateAIModule(GameChoice opponentChoice)
    {
        if(transitionMatrix.ContainsKey(opponentLastChoice) && transitionMatrix[opponentLastChoice].ContainsKey(opponentChoice))
        {
            transitionMatrix[opponentLastChoice][opponentChoice]++;
        }
        opponentLastChoice = opponentChoice;
    }
}
