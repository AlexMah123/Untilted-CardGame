using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtilsLibrary
{
    public static GameResult GetGameResult(GameChoice humanPlayerChoice, GameChoice aiPlayerChoice)
    {
        var result = GameResult.NONE;

        if (humanPlayerChoice == aiPlayerChoice)
        {
            result = GameResult.DRAW;
        }
        else if (humanPlayerChoice == GameChoice.ROCK && aiPlayerChoice == GameChoice.SCISSOR ||
                humanPlayerChoice == GameChoice.PAPER && aiPlayerChoice == GameChoice.ROCK ||
                humanPlayerChoice == GameChoice.SCISSOR && aiPlayerChoice == GameChoice.PAPER)
        {
            result = GameResult.WIN;
        }
        else
        {
            result = GameResult.LOSE;
        }

        return result;
    }

    public static T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length));
        return V;
    }
}
