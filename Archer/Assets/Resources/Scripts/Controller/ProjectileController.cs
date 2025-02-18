using ArcheroClone.Model;
using ArcheroClone.View;
using UnityEngine;

namespace ArcheroClone.Controller
{
    public class ProjectileController : MonoBehaviour
    {
        private ProjectileModel projectileModel;
        private ProjectileView projectileView;
        
        private void Awake()
        {
            projectileModel = GetComponent<ProjectileModel>();
            projectileView = GetComponent<ProjectileView>();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            bool hitTarget = false;
            
            // Check if player projectile hit enemy
            if (!projectileModel.isEnemyProjectile && other.CompareTag("Enemy"))
            {
                EnemyModel enemyModel = other.GetComponent<EnemyModel>();
                if (enemyModel != null)
                {
                    enemyModel.TakeDamage(projectileModel.damage);
                    hitTarget = true;
                }
            }
            // Check if enemy projectile hit player
            else if (projectileModel.isEnemyProjectile && other.CompareTag("Player"))
            {
                PlayerModel playerModel = other.GetComponent<PlayerModel>();
                if (playerModel != null)
                {
                    playerModel.TakeDamage(projectileModel.damage);
                    hitTarget = true;
                }
            }
            // Check if projectile hit wall or obstacle
            else if (other.CompareTag("Wall") || other.CompareTag("Obstacle"))
            {
                hitTarget = true;
            }
            
            if (hitTarget)
            {
                // Play impact effect
                if (projectileView != null)
                {
                    projectileView.PlayImpactEffect();
                }
                
                // Destroy the projectile
                Destroy(gameObject);
            }
        }
    }
}