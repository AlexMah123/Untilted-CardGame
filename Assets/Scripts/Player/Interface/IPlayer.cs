using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    public PlayerStatsSO StatsConfig { get;}
    public StatComponent StatComponent { get; }
    public ChoiceComponent ChoiceComponent { get; }
    public ActiveLoadoutComponent ActiveLoadoutComponent { get; }

    public GameChoice GetChoice();
}
