using System;
using UnityEngine;

public class InspectComponent : MonoBehaviour
{
    public static event Action<UpgradeDefinitionSO> OnCardInspected;

    public static void InspectCard(UpgradeDefinitionSO upgrade)
    {
        OnCardInspected?.Invoke(upgrade);
    }
}
