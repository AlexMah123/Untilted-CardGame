using System.Collections.Generic;
using PlayerCore.PlayerComponents;

namespace PlayerCore.AIPlayer
{
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
                case GameChoice.Rock: return GameChoice.Paper;
                case GameChoice.Paper: return GameChoice.Scissor;
                case GameChoice.Scissor: return GameChoice.Rock;

                //shouldnt happen
                default: return GameChoice.Rock;
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
}