using System;
using UnityEngine;

namespace PlayerCore.PlayerComponents
{
    public class HealthComponent : MonoBehaviour
    {
        [Header("Runtime Value")] 
        public int maxHealth;
        public int currentHealth;

        //declaration of events
        public event Action<int> OnHealthModified;
        public event Action OnHealthZero;

        public void InitializeComponent(PlayerStats referencedStats)
        {
            maxHealth = referencedStats.maxHealth;
            currentHealth = maxHealth;

            currentHealth = Mathf.Min(currentHealth, maxHealth);

            OnHealthModified?.Invoke(currentHealth);
        }

        public void UpdateComponent(PlayerStats referencedStats)
        {
            maxHealth = referencedStats.maxHealth;
            currentHealth = Mathf.Min(currentHealth, maxHealth);

            OnHealthModified?.Invoke(currentHealth);
        }

        public void SetHealth(int value)
        {
            ModifyHealthAmount(value - currentHealth);
        }

        public void IncreaseHealth(int value)
        {
            ModifyHealthAmount(value);
        }

        public void DecreaseHealth(int value)
        {
            ModifyHealthAmount(-1 * value);
        }

        #region Internal Function

        private void ModifyHealthAmount(int value)
        {
            currentHealth += value;

            currentHealth = Mathf.Min(currentHealth, maxHealth);

            OnHealthModified?.Invoke(currentHealth);

            if (currentHealth <= 0)
            {
                OnHealthZero?.Invoke();
            }
        }

        #endregion
    }
}