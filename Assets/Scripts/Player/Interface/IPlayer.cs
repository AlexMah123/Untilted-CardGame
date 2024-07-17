using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    public ChoiceComponent ChoiceComponent { get; }
    public ActiveLoadoutComponent ActiveLoadoutComponent { get; }
    public HealthComponent HealthComponent { get; }
    public DamageComponent DamageComponent { get; }
    public EnergyComponent EnergyComponent { get; }

    public GameChoice GetChoice();
}
