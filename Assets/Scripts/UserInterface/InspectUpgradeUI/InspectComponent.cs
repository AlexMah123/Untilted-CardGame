using System;
using PlayerCore.Upgrades.Base;
using UnityEngine;

namespace UserInterface.InspectUpgradeUI
{
    public class InspectComponent : MonoBehaviour
    {
        public static event Action<UpgradeDefinitionSO> OnCardInspected;

        public static void InspectCard(UpgradeDefinitionSO upgrade)
        {
            OnCardInspected?.Invoke(upgrade);
        }
    }
}