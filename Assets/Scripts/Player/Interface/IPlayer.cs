using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    public PlayerStatsSO StatsConfig { get;}
    public StatComponent StatComponent { get; }

    public void GetChoice();
}
