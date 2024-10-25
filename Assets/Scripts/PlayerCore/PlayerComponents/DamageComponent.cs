using System;
using UnityEngine;

namespace PlayerCore.PlayerComponents
{
    public class DamageComponent : MonoBehaviour
    {
        [Header("Runtime Value")] 
        public int damageAmount;

        //declaration of events
        public event Action<int> OnDamageModified;

        public void InitializeComponent(PlayerStats referencedStats)
        {
            damageAmount = referencedStats.damage;

            OnDamageModified?.Invoke(damageAmount);
        }

        public void IncreaseDamage(int value)
        {
            ModifyDamageAmount(value);
        }

        public void DecreaseDamage(int value)
        {
            ModifyDamageAmount(-1 * value);
        }

        public void DealDamage(Player target, int amount)
        {
            target.HealthComponent.DecreaseHealth(amount);
        }

        #region Internal Function

        private void ModifyDamageAmount(int value)
        {
            damageAmount += value;

            //#TODO: broadcast event to total stats display
            OnDamageModified?.Invoke(damageAmount);
        }

        #endregion
    }
}