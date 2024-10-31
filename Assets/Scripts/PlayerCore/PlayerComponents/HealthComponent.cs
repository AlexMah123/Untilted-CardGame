using System;
using UnityEngine;

namespace PlayerCore.PlayerComponents
{
    public class HealthComponent : MonoBehaviour
    {
        [Header("Runtime Value")] 
        public int currentHealth;

        private int bonusHealth = 0;

        //declaration of events
        public event Action<int> OnHealthModified;
        public event Action OnHealthZero;

        public void InitializeComponent(PlayerStats referencedStats, int effectiveBonusHealth)
        {
            bonusHealth = effectiveBonusHealth;
            SetHealth(referencedStats.maxHealth);
        }

        public void UpdateComponent(PlayerStats referencedStats, int effectiveBonusHealth)
        {
            //check if the the bonus has changed
            if (bonusHealth != effectiveBonusHealth)
            {
                int healthDelta = effectiveBonusHealth - bonusHealth;
                
                //update the difference
                ModifyHealthAmount(healthDelta);
                
                //update bonusHealth
                bonusHealth = effectiveBonusHealth;
            }
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

            currentHealth = Mathf.Max(currentHealth, 0);

            OnHealthModified?.Invoke(currentHealth);

            if (currentHealth <= 0)
            {
                OnHealthZero?.Invoke();
            }
        }

        #endregion
    }
}