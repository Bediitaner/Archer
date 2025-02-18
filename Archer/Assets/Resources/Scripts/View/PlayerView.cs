// PlayerView.cs
using UnityEngine;
using ArcheroClone.Model;

namespace ArcheroClone.View
{
    public class PlayerView : MonoBehaviour
    {
        public Animator animator;
        public SpriteRenderer spriteRenderer;
        public Transform projectileSpawnPoint;
        public GameObject projectilePrefab;
        public GameObject levelUpEffectPrefab;
        
        private PlayerModel playerModel;
        
        private void Awake()
        {
            playerModel = GetComponent<PlayerModel>();
            
            if (animator == null)
                animator = GetComponent<Animator>();
                
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        private void Start()
        {
            playerModel.OnLevelUp += PlayLevelUpEffect;
            playerModel.OnPlayerDied += PlayDeathAnimation;
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
        
        private void PlayDeathAnimation()
        {
            if (animator == null) return;
            
            animator.SetTrigger("Die");
        }
        
        private void PlayLevelUpEffect(int level)
        {
            if (levelUpEffectPrefab != null)
            {
                Instantiate(levelUpEffectPrefab, transform.position, Quaternion.identity);
            }
        }
        
        public void FireProjectile(Vector2 targetPosition)
        {
            if (projectilePrefab == null || projectileSpawnPoint == null) return;
            
            Vector2 direction = (targetPosition - (Vector2)projectileSpawnPoint.position).normalized;
            
            GameObject newProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            
            ProjectileModel projectileModel = newProjectile.GetComponent<ProjectileModel>();
            projectileModel.damage = playerModel.stats.damage;
            projectileModel.speed = playerModel.stats.projectileSpeed;
            
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