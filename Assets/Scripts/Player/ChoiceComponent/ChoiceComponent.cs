using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameChoice
{
    NONE,
    ROCK,
    PAPER,
    SCISSOR
}

public class ChoiceComponent : MonoBehaviour
{
    public event Action OnSealChoiceEvent;

    public Dictionary<GameChoice, bool> ChoicesAvailable = new Dictionary<GameChoice, bool> {
        { GameChoice.ROCK, true },
        { GameChoice.PAPER, true },
        { GameChoice.SCISSOR, true },
    };

    public void SealChoice(GameChoice choice)
    {
        ChoicesAvailable[choice] = false;

        //#TODO: broadcast event to PlayerHandUIManager to update
        OnSealChoiceEvent?.Invoke();
    }

    public void ResetChoicesAvailable()
    {
        foreach(var key in ChoicesAvailable.Keys)
        {
            ChoicesAvailable[key] = true;
        }
    }
}
