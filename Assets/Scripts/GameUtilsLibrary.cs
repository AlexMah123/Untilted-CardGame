using GameCore;
using PlayerCore.PlayerComponents;

public static class GameUtilsLibrary
{
    public static GameResult GetPlayerGameResult(GameChoice humanPlayerChoice, GameChoice aiPlayerChoice)
    {
        var result = GameResult.None;

        if (humanPlayerChoice == aiPlayerChoice)
        {
            result = GameResult.Draw;
        }
        else if (humanPlayerChoice == GameChoice.Rock && aiPlayerChoice == GameChoice.Scissor ||
                 humanPlayerChoice == GameChoice.Paper && aiPlayerChoice == GameChoice.Rock ||
                 humanPlayerChoice == GameChoice.Scissor && aiPlayerChoice == GameChoice.Paper)
        {
            result = GameResult.Win;
        }
        else
        {
            result = GameResult.Lose;
        }

        return result;
    }

    public static GameResult ConvertToEnemyResult(GameResult playerResult)
    {
        switch (playerResult)
        {
            case GameResult.Win: return GameResult.Lose;
            case GameResult.Lose: return GameResult.Win;
            case GameResult.Draw: return GameResult.Draw;
            
            //this should not happen
            default: return GameResult.None;
        }
    }

    public static T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length));
        return V;
    }
}