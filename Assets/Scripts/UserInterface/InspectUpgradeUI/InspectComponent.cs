using System;
using UnityEngine;

using Upgrades.Base;

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
