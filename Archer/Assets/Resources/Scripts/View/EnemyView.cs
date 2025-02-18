using ArcheroClone.Model;
using UnityEngine;

namespace ArcheroClone.View
{
    public class EnemyView : MonoBehaviour
    {
        public Animator animator;
        public SpriteRenderer spriteRenderer;
        public Transform projectileSpawnPoint;
        public GameObject projectilePrefab;
        public GameObject deathEffectPrefab;
        public GameObject healthBarPrefab;
        public Transform healthBarAnchor;
        
        private EnemyModel enemyModel;
        private GameObject healthBarInstance;
        
        private void Awake()
        {
            enemyModel = GetComponent<EnemyModel>();
            
            if (animator == null)
                animator = GetComponent<Animator>();
                
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();
                
            if (healthBarPrefab != null && healthBarAnchor != null)
            {
                healthBarInstance = Instantiate(healthBarPrefab, healthBarAnchor);
            }
        }
        
        private void Start()
        {
            enemyModel.OnHealthChanged += UpdateHealthBar;
            enemyModel.OnEnemyDied += PlayDeathEffect;
        }
        
        public void PlayMoveAnimation(Vector2 direction)
        {
            if (animator == null) return;
            
            bool isMoving = direction.magnitude > 0.1f;
            animator.SetBool("IsMoving", isMoving);
            
            if (isMoving)
            {
                // Set direction for animation (facing left or right)
                if (direction.x != 0)
                {
                    spriteRenderer.flipX = direction.x < 0;
                }
            }
        }
        
        public void PlayAttackAnimation()
        {
            if (animator == null) return;
            
            animator.SetTrigger("Attack");
        }
        
        public void PlayHitAnimation()
        {
            if (animator == null) return;
            
            animator.SetTrigger("Hit");
        }
        
        private void PlayDeathEffect()
        {
            if (deathEffectPrefab != null)
            {
                Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            }
        }
        
        private void UpdateHealthBar(int currentHealth, int maxHealth)
        {
            if (healthBarInstance == null) return;
            
            float healthPercent = (float)currentHealth / maxHealth;
            HealthBarView healthBar = healthBarInstance.GetComponent<HealthBarView>();
            if (healthBar != null)
            {
                healthBar.UpdateHealthBar(healthPercent);
            }
        }
        
        public void FireProjectile(Vector2 targetPosition)
        {
            if (projectilePrefab == null || projectileSpawnPoint == null || !enemyModel.stats.isRanged) return;
            
            Vector2 direction = (targetPosition - (Vector2)projectileSpawnPoint.position).normalized;
            
            GameObject newProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            
            ProjectileModel projectileModel = newProjectile.GetComponent<ProjectileModel>();
            projectileModel.damage = enemyModel.stats.damage;
            projectileModel.speed = enemyModel.stats.projectileSpeed;
            projectileModel.isEnemyProjectile = true;
            
            Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * projectileModel.speed;
                
                // Rotate projectile to face direction of movement
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                newProjectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            
            PlayAttackAnimation();
        }
    }
}