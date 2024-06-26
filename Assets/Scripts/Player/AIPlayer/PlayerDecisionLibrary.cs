using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerDecisionLibrary 
{
    #region Helper
    public static T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length));
        return V;
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

    #endregion
}
