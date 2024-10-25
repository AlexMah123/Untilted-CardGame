using TMPro;
using UnityEngine;
using PlayerCore;

namespace UserInterface.StatsUI
{
    public class EnergyStatUI : MonoBehaviour
    {
        public Player attachedPlayer;
        [SerializeField] TextMeshProUGUI energyText;

        private void Start()
        {
            if (attachedPlayer != null)
            {
                attachedPlayer.EnergyComponent.OnEnergyModified += HandleOnEnergyModified;
            }
            else
            {
                throw new System.AccessViolationException("Attached Player is null");
            }

            //initial call to set values
            HandleOnEnergyModified(attachedPlayer.EnergyComponent.energyAmount);
        }

        private void HandleOnEnergyModified(int newHealth)
        {
            energyText.text = newHealth.ToString();
        }
    }
}