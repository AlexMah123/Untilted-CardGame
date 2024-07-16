using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    public ChoiceComponent ChoiceComponent { get; }
    public ActiveLoadoutComponent ActiveLoadoutComponent { get; }
    public HealthStatComponent HealthStatComponent { get; }
    public DamageStatComponent DamageStatComponent { get; }

    public GameChoice GetChoice();
}
