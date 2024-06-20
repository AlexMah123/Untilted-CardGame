using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DecisionSO", menuName = "Enemy/Decision/BayesianModelSO")]
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

        if(rockProbability >= paperProbability && rockProbability >= scissorsProbability)
        {
            //player most probability throw rock
            aiDecision = GameChoice.PAPER;
        }
        else if(paperProbability >= rockProbability && paperProbability >= scissorsProbability)
        {
            //player most probability throw paper
            aiDecision = GameChoice.SCISSOR;
        }
        else
        {
            //player most probability throw scissors
            aiDecision = GameChoice.ROCK;
        }

        return aiDecision;
    }

    public override void UpdateAIModule(GameChoice opponentChoice)
    {
        if (opponentMoveCount.ContainsKey(opponentChoice))
        {
            opponentMoveCount[opponentChoice]++;
        }
    }
}
