using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlayerCore.PlayerComponents
{
    public enum GameChoice
    {
        Rock,
        Paper,
        Scissor
    }

    public class ChoiceComponent : MonoBehaviour
    {
        [Header("Runtime Values")]
        public GameChoice currentChoice = GameChoice.Rock;

        //events
        public event Action OnChoiceSealed;

        public Dictionary<GameChoice, bool> choicesAvailable = new Dictionary<GameChoice, bool> {
            { GameChoice.Rock, true },
            { GameChoice.Paper, true },
            { GameChoice.Scissor, true },
        };

        public bool IsChoiceAvailable(GameChoice choice)
        {
            return choicesAvailable[choice];
        }

        public void SealChoice(GameChoice choice)
        {
            choicesAvailable[choice] = false;

            //broadcast event to PlayerHandUIManager to update
            OnChoiceSealed?.Invoke();
        }

        [ContextMenu("ChoiceComponent/ResetChoices")]
        public void ResetChoicesAvailable()
        {
            var choicesKVP = choicesAvailable.ToList();
            foreach (var choice in choicesKVP)
            {
                choicesAvailable[choice.Key] = true;
            }

            //broadcast event to PlayerHandUIManager to update
            OnChoiceSealed?.Invoke();
        }

        public List<GameChoice> FetchAllChoicesAvailable()
        {
            //query through the dictionary to get all game choices that are true, and return the key.
            List<GameChoice> queriedChoices = choicesAvailable.Where(x => x.Value == true).Select(x => x.Key).ToList();
            return queriedChoices;
        }

#if UNITY_EDITOR
        [ContextMenu("ChoiceComponent/SealRock")]
        public void SealRock()
        {
            SealChoice(GameChoice.Rock);
        }

        [ContextMenu("ChoiceComponent/SealPaper")]
        public void SealPaper()
        {
            SealChoice(GameChoice.Paper);
        }

        [ContextMenu("ChoiceComponent/SealScissor")]
        public void SealScissor()
        {
            SealChoice(GameChoice.Scissor);
        }
#endif
    }
}