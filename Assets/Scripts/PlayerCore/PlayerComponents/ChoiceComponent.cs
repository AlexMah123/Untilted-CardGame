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

        public Dictionary<GameChoice, bool> choicesAvailable = new Dictionary<GameChoice, bool>
        {
            { GameChoice.Rock, true },
            { GameChoice.Paper, true },
            { GameChoice.Scissor, true },
        };

        public Dictionary<GameChoice, int> choicesSealed = new Dictionary<GameChoice, int>
        {
            { GameChoice.Rock, 0 },
            { GameChoice.Paper, 0 },
            { GameChoice.Scissor, 0 },
        };

        public bool IsChoiceAvailable(GameChoice choice)
        {
            return choicesAvailable[choice];
        }

        public void SealChoice(GameChoice choice, int duration)
        {
            choicesAvailable[choice] = false;
            choicesSealed[choice] = duration;

            //broadcast event to PlayerHandUIManager to update
            OnChoiceSealed?.Invoke();
        }

        [ContextMenu("ChoiceComponent/ResetChoices")]
        public void ResetChoicesAvailable()
        {
            //loop through choicesSealed and decrement the duration.
            var choicesSealedKVP = choicesSealed.ToList();
            foreach (var choice in choicesSealedKVP)
            {
                choicesSealed[choice.Key] = Mathf.Max(choicesSealed[choice.Key] - 1, 0);

                //if the duration is finished, unseal it (set it to available/true)
                if (choicesSealed[choice.Key] == 0)
                {
                    choicesAvailable[choice.Key] = true;
                }
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
            SealChoice(GameChoice.Rock, 1);
        }

        [ContextMenu("ChoiceComponent/SealPaper")]
        public void SealPaper()
        {
            SealChoice(GameChoice.Paper, 1);
        }

        [ContextMenu("ChoiceComponent/SealScissor")]
        public void SealScissor()
        {
            SealChoice(GameChoice.Scissor, 1);
        }
#endif
    }
}