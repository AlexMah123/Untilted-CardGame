using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerDecisionLibrary 
{
    public static GameChoice GetRandomGameChoice()
    {
        System.Array A = System.Enum.GetValues(typeof(GameChoice));
        GameChoice choice = (GameChoice)A.GetValue(UnityEngine.Random.Range(0, A.Length));
        return choice;
    }

    public static GameChoice GetCounterChoice(GameChoice predictedMoveByOpponent)
    {
        switch (predictedMoveByOpponent)
        {
            case GameChoice.ROCK:           return GameChoice.PAPER;
            case GameChoice.PAPER:          return GameChoice.SCISSOR;
            case GameChoice.SCISSOR:        return GameChoice.ROCK;

            //shouldnt happen
            default:                        return GameChoice.ROCK;
        }
    }

    public static List<GameChoice> GetPlayerAvailableChoices(IPlayer targetedPlayer)
    {
        return targetedPlayer.ChoiceComponent.FetchAllChoicesAvailable();
    }

    public static GameChoice GetPredictedChoice(IPlayer targetedPlayer)
    {
        return targetedPlayer.GetChoice();
    }
}
