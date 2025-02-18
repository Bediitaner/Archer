using UnityEngine;

namespace ArcheroClone.Model
{
    public class PlayerModel : MonoBehaviour
    {
        public PlayerStats stats;
        public System.Action<int, int> OnHealthChanged;
        public System.Action<int> OnExperienceGained;
        public System.Action<int> OnLevelUp;
        public System.Action<int> OnGoldChanged;
        public System.Action OnPlayerDied;

        private void Awake()
        {
            InitializeStats();
        }

        private void InitializeStats()
        {
            stats.currentHealth = stats.maxHealth;
        }

        public void TakeDamage(int damage)
        {
            stats.currentHealth -= damage;
            if (stats.currentHealth <= 0)
            {
                stats.currentHealth = 0;
                Die();
            }
            OnHealthChanged?.Invoke(stats.currentHealth, stats.maxHealth);
        }

        public void Heal(int amount)
        {
            stats.currentHealth = Mathf.Min(stats.currentHealth + amount, stats.maxHealth);
            OnHealthChanged?.Invoke(stats.currentHealth, stats.maxHealth);
        }

        public void GainExperience(int amount)
        {
            stats.experience += amount;
            OnExperienceGained?.Invoke(amount);

            while (stats.experience >= stats.experienceToNextLevel)
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            stats.experience -= stats.experienceToNextLevel;
            stats.level++;
            stats.experienceToNextLevel = 100 * stats.level;
            
            // Improve stats on level up
            stats.maxHealth += 10;
            stats.currentHealth = stats.maxHealth;  // Full heal on level up
            stats.damage += 2;
            stats.moveSpeed += 0.1f;
            
            OnLevelUp?.Invoke(stats.level);
            OnHealthChanged?.Invoke(stats.currentHealth, stats.maxHealth);
        }

        public void CollectGold(int amount)
        {
            stats.goldCoins += amount;
            OnGoldChanged?.Invoke(stats.goldCoins);
        }

        private void Die()
        {
            OnPlayerDied?.Invoke();
        }
    }
}