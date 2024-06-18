using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatComponent : MonoBehaviour
{
    [Header("Stats Component Config")]
    public PlayerStatsSO stats;

    public Dictionary<GameChoice, bool> ChoicesAvailable = new Dictionary<GameChoice, bool> { 
        { GameChoice.ROCK, true },
        { GameChoice.PAPER, true },
        { GameChoice.SCISSOR, true },
    };

    public void SealChoice(GameChoice choice)
    {
        ChoicesAvailable[choice] = false;
    }

    public void IncreaseHealth(int value)
    {
        ModifyHealth(value);
    }

    public void DecreaseHealth(int value)
    {
        ModifyHealth(-1 * value);
    }

    #region Internal Function
    private void ModifyHealth(int value)
    {
        stats.health += value;
    }
    #endregion
}
