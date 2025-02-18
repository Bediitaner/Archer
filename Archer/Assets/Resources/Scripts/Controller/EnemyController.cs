using ArcheroClone.Model;
using ArcheroClone.View;
using UnityEngine;

namespace ArcheroClone.Controller
{
    public class EnemyController : MonoBehaviour
    {
        private EnemyModel enemyModel;
        private EnemyView enemyView;
        private Rigidbody2D rb;

        private Transform player;
        private float attackTimer;

        private void Awake()
        {
            enemyModel = GetComponent<EnemyModel>();
            enemyView = GetComponent<EnemyView>();
            rb = GetComponent<Rigidbody2D>();

            // Find the player
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        private void Start()
        {
            // Register enemy with game model
            if (GameModel.Instance != null)
            {
                GameModel.Instance.SetEnemiesRemaining(FindObjectsOfType<EnemyController>().Length);
            }

            // Subscribe to enemy death event
            enemyModel.OnEnemyDied += OnEnemyDied;
        }

        private void Update()
        {
            // Don't update if game is over or player is dead
            if (GameModel.Instance.stats.isGameOver || GameModel.Instance.stats.isPaused || player == null) return;

            // Calculate distance to player
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Update attack timer
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }

            // Handle enemy behavior based on type and distance
            if (enemyModel.stats.isRanged && distanceToPlayer <= enemyModel.stats.attackRange)
            {
                // Ranged enemy in attack range - stop and attack
                StopMoving();
                // EnemyController.cs (continued from the previous artifact)
                if (attackTimer <= 0)
                {
                    attackTimer = 1f / enemyModel.stats.attackRate;
                    RangedAttack();
                }
            }
            else if (!enemyModel.stats.isRanged && distanceToPlayer <= enemyModel.stats.attackRange)
            {
                // Melee enemy in attack range - stop and attack
                StopMoving();
                if (attackTimer <= 0)
                {
                    attackTimer = 1f / enemyModel.stats.attackRate;
                    MeleeAttack();
                }
            }
            else
            {
                // Move towards player
                MoveTowardsPlayer();
            }
        }

        private void MoveTowardsPlayer()
        {
            if (player == null || rb == null) return;

            // Calculate direction to player
            Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;

            // Move towards player
            rb.velocity = direction * enemyModel.stats.moveSpeed;

            // Update animation
            enemyView.PlayMoveAnimation(direction);
        }

        private void StopMoving()
        {
            if (rb == null) return;

            rb.velocity = Vector2.zero;
            enemyView.PlayMoveAnimation(Vector2.zero);
        }

        private void MeleeAttack()
        {
            if (player == null) return;

            // Check if player is still in range (could have moved)
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= enemyModel.stats.attackRange)
            {
                // Deal damage to player
                PlayerModel playerModel = player.GetComponent<PlayerModel>();
                if (playerModel != null)
                {
                    playerModel.TakeDamage(enemyModel.stats.damage);
                }

                // Play attack animation
                enemyView.PlayAttackAnimation();
            }
        }

        private void RangedAttack()
        {
            if (player == null) return;

            // Fire projectile toward player
            enemyView.FireProjectile(player.position);
        }

        private void OnEnemyDied()
        {
            // Update game model
            if (GameModel.Instance != null)
            {
                GameModel.Instance.EnemyKilled();
            }

            // Spawn rewards
            SpawnRewards();
        }

        private void SpawnRewards()
        {
            // Spawn gold
            if (Random.value < 0.7f) // 70% chance to drop gold
            {
                // Get prefab from LevelController
                GameObject goldPrefab = LevelController.Instance.goldPrefab;
                if (goldPrefab != null)
                {
                    GameObject gold = Instantiate(goldPrefab, transform.position, Quaternion.identity);
                    GoldPickup goldPickup = gold.GetComponent<GoldPickup>();
                    if (goldPickup != null)
                    {
                        goldPickup.goldAmount = enemyModel.stats.goldValue;
                    }
                }
            }

            // Spawn experience orb (always)
            GameObject expPrefab = LevelController.Instance.experienceOrbPrefab;
            if (expPrefab != null)
            {
                GameObject exp = Instantiate(expPrefab, transform.position, Quaternion.identity);
                ExperienceOrb expOrb = exp.GetComponent<ExperienceOrb>();
                if (expOrb != null)
                {
                    expOrb.experienceAmount = enemyModel.stats.experienceValue;
                }
            }

            // Spawn health potion (rare)
            if (Random.value < 0.1f) // 10% chance to drop health potion
            {
                GameObject healthPotionPrefab = LevelController.Instance.healthPotionPrefab;
                if (healthPotionPrefab != null)
                {
                    Instantiate(healthPotionPrefab, transform.position, Quaternion.identity);
                }
            }
        }
    }
}

