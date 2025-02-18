using ArcheroClone.Controller;
using ArcheroClone.Model;
using ArcheroClone.View;
using UnityEngine;

namespace ArcheroClone.Factory
{
    public class EnemyFactory : MonoBehaviour
    {
        public static GameObject CreateEnemy(GameObject enemyPrefab, Vector3 position, int levelMultiplier = 1)
        {
            GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
            
            // Ensure the enemy has all required components
            if (enemy.GetComponent<EnemyModel>() == null)
                enemy.AddComponent<EnemyModel>();
                
            if (enemy.GetComponent<EnemyView>() == null)
                enemy.AddComponent<EnemyView>();
                
            if (enemy.GetComponent<EnemyController>() == null)
                enemy.AddComponent<EnemyController>();
                
            if (enemy.GetComponent<Rigidbody2D>() == null)
            {
                Rigidbody2D rb = enemy.AddComponent<Rigidbody2D>();
                rb.gravityScale = 0f;
                rb.freezeRotation = true;
            }
            
            if (enemy.GetComponent<CircleCollider2D>() == null)
            {
                CircleCollider2D collider = enemy.AddComponent<CircleCollider2D>();
                collider.isTrigger = false;
            }
            
            // Tag the enemy
            enemy.tag = "Enemy";
            
            // Scale enemy stats based on level
            ScaleEnemyStats(enemy, levelMultiplier);
            
            return enemy;
        }
        
        private static void ScaleEnemyStats(GameObject enemy, int levelMultiplier)
        {
            if (levelMultiplier <= 1) return;
            
            EnemyModel model = enemy.GetComponent<EnemyModel>();
            if (model != null)
            {
                // Scale health based on level
                model.stats.maxHealth = (int)(model.stats.maxHealth * (1 + 0.2f * (levelMultiplier - 1)));
                model.stats.currentHealth = model.stats.maxHealth;
                
                // Scale damage based on level
                model.stats.damage = (int)(model.stats.damage * (1 + 0.1f * (levelMultiplier - 1)));
                
                // Scale experience and gold value
                model.stats.experienceValue = (int)(model.stats.experienceValue * (1 + 0.15f * (levelMultiplier - 1)));
                model.stats.goldValue = (int)(model.stats.goldValue * (1 + 0.15f * (levelMultiplier - 1)));
            }
        }
    }
}