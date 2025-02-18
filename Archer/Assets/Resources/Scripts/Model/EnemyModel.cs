using UnityEngine;

namespace ArcheroClone.Model
{
    public class EnemyModel : MonoBehaviour
    {
        public EnemyStats stats;
        public System.Action<int, int> OnHealthChanged;
        public System.Action OnEnemyDied;

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

        private void Die()
        {
            OnEnemyDied?.Invoke();
            Destroy(gameObject);
        }
    }
}