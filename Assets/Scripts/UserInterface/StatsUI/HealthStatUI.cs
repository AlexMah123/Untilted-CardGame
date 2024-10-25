using TMPro;
using UnityEngine;

using PlayerCore;

namespace UserInterface.StatsUI
{
    public class HealthStatUI : MonoBehaviour
    {
        public Player attachedPlayer;
        [SerializeField] TextMeshProUGUI healthText;

        private void OnDisable()
        {
            if (attachedPlayer != null)
            {
                attachedPlayer.HealthComponent.OnHealthModified -= HandleOnHealthModified;
            }
        }

        private void Start()
        {
            if (attachedPlayer != null)
            {
                attachedPlayer.HealthComponent.OnHealthModified += HandleOnHealthModified;
            }
            else
            {
                throw new System.AccessViolationException("Attached Player is null");
            }

            //initial call to set values
            HandleOnHealthModified(attachedPlayer.HealthComponent.currentHealth);
        }

        private void HandleOnHealthModified(int newHealth)
        {
            healthText.text = newHealth.ToString();
        }
    }
}
