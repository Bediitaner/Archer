// PlayerController.cs
using UnityEngine;
using ArcheroClone.Model;
using ArcheroClone.View;

namespace ArcheroClone.Controller
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerModel playerModel;
        private PlayerView playerView;
        private Rigidbody2D rb;
        
        private Vector2 moveDirection;
        private Transform nearestEnemy;
        private float attackTimer;
        private bool isAttacking;
        
        private void Awake()
        {
            playerModel = GetComponent<PlayerModel>();
            playerView = GetComponent<PlayerView>();
            rb = GetComponent<Rigidbody2D>();
        }
        
        private void Update()
        {
            // Don't update if game is over
            if (GameModel.Instance.stats.isGameOver || GameModel.Instance.stats.isPaused) return;
            
            // Get input
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector2(moveX, moveY).normalized;
            
            // Handle player animation
            playerView.PlayMoveAnimation(moveDirection);
            
            // Attack logic
            if (moveDirection.magnitude > 0.1f)
            {
                // Moving, so stop attacking
                isAttacking = false;
            }
            else
            {
                // Find nearest enemy
                nearestEnemy = FindNearestEnemy();
                if (nearestEnemy != null)
                {
                    isAttacking = true;
                    // Update attack timer
                    attackTimer -= Time.deltaTime;
                    if (attackTimer <= 0)
                    {
                        attackTimer = 1f / playerModel.stats.attackSpeed;
                        Attack();
                    }
                }
                else
                {
                    isAttacking = false;
                }
            }
        }
        
        private void FixedUpdate()
        {
            // Handle movement
            if (!isAttacking && rb != null)
            {
                rb.velocity = moveDirection * playerModel.stats.moveSpeed;
            }
            else if (rb != null)
            {
                rb.velocity = Vector2.zero;
            }
        }
        
        private Transform FindNearestEnemy()
        {
            EnemyController[] enemies = FindObjectsOfType<EnemyController>();
            
            if (enemies.Length == 0) return null;
            
            Transform nearest = null;
            float closestDistance = float.MaxValue;
            
            foreach (EnemyController enemy in enemies)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearest = enemy.transform;
                }
            }
            
            return nearest;
        }
        
        private void Attack()
        {
            if (nearestEnemy != null)
            {
                // Fire projectile toward the nearest enemy
                playerView.FireProjectile(nearestEnemy.position);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            // Handle collectibles
            if (other.CompareTag("Gold"))
            {
                GoldPickup goldPickup = other.GetComponent<GoldPickup>();
                if (goldPickup != null)
                {
                    playerModel.CollectGold(goldPickup.goldAmount);
                    Destroy(other.gameObject);
                }
            }
            else if (other.CompareTag("HealthPotion"))
            {
                HealthPotion healthPotion = other.GetComponent<HealthPotion>();
                if (healthPotion != null)
                {
                    playerModel.Heal(healthPotion.healAmount);
                    Destroy(other.gameObject);
                }
            }
            else if (other.CompareTag("ExperienceOrb"))
            {
                ExperienceOrb expOrb = other.GetComponent<ExperienceOrb>();
                if (expOrb != null)
                {
                    playerModel.GainExperience(expOrb.experienceAmount);
                    Destroy(other.gameObject);
                }
            }
        }
    }
}