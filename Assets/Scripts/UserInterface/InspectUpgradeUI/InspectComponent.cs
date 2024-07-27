using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectComponent : MonoBehaviour
{
    public static event Action<UpgradeDefinitionSO> OnCardInspectedEvent;

    public static void InspectCard(UpgradeDefinitionSO upgrade)
    {
        OnCardInspectedEvent?.Invoke(upgrade);
    }
}
