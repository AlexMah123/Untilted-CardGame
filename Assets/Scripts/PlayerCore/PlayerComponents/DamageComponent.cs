using System;
using UnityEngine;

namespace PlayerCore.PlayerComponents
{
    public class DamageComponent : MonoBehaviour
    {
        [Header("Runtime Value")] 
        public int attack;

        public int damageTaken;

        //declaration of events
        public event Action<int> OnAttackModified;
        public event Action<int> OnDamageTakenModified;

        public void InitializeComponent(PlayerStats referencedStats)
        {
            attack = referencedStats.attack;
            damageTaken = referencedStats.damageTaken;
            
            OnAttackModified?.Invoke(attack);
            OnDamageTakenModified?.Invoke(damageTaken);
        }

        public void IncreaseAttack(int value)
        {
            ModifyAttack(value);
        }

        public void DecreaseAttack(int value)
        {
            ModifyAttack(-1 * value);
        }
        
        public void IncreaseDamageTaken(int value)
        {
            ModifyDamageTaken(value);
        }

        public void DecreaseDamageTaken(int value)
        {
            ModifyDamageTaken(-1 * value);
        }
        
        
        public void DealDamage(Player target, int attackAmount)
        {
            //the amount + how much more damage target takes.
            int totalDamage = attackAmount + target.DamageComponent.damageTaken;

            if (totalDamage > 0)
            {
                target.HealthComponent.DecreaseHealth(totalDamage);
            }
        }

        #region Internal Function
        private void ModifyAttack(int value)
        {
            attack += value;
            attack = Mathf.Min(attack, 0);
            
            //#TODO: broadcast event to total stats display
            OnAttackModified?.Invoke(attack);
        }
        
        private void ModifyDamageTaken(int value)
        {
            damageTaken += value;
            damageTaken = Mathf.Min(damageTaken, 0);
            
            //#TODO: broadcast event to total stats display
            OnAttackModified?.Invoke(damageTaken);
        }
        #endregion
    }
}