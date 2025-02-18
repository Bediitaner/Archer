using System.Collections.Generic;
using ArcheroClone.Factory;
using ArcheroClone.Model;
using UnityEngine;

namespace ArcheroClone.Managers
{
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager Instance { get; private set; }
        
        [Header("Enemy Spawning")]
        public List<GameObject> enemyPrefabs;
        public Transform[] spawnPoints;
        public int baseEnemiesPerLevel = 5;
        public float enemiesPerLevelMultiplier = 1.5f;
        public float minSpawnDelay = 0.5f;
        public float maxSpawnDelay = 2.0f;
        
        [Header("Difficulty Scaling")]
        public float healthMultiplierPerLevel = 1.2f;
        public float damageMultiplierPerLevel = 1.1f;
        public float speedMultiplierPerLevel = 1.05f;
        
        private List<GameObject> activeEnemies = new List<GameObject>();
        private GameModel gameModel;
        private int currentLevel = 1;
        private int remainingEnemies = 0;
        
        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            // Find game model
            gameModel = GameModel.Instance;
        }
        
        private void Start()
        {
            // Subscribe to events
            if (gameModel != null)
            {
                gameModel.OnLevelChanged += HandleLevelChanged;
                
                // Set initial level
                currentLevel = gameModel.stats.currentLevel;
            }
            
            // Initialize spawn points if not set
            if (spawnPoints == null || spawnPoints.Length == 0)
            {
                GameObject spawnPointsParent = GameObject.Find("SpawnPoints");
                if (spawnPointsParent != null)
                {
                    List<Transform> points = new List<Transform>();
                    foreach (Transform child in spawnPointsParent.transform)
                    {
                        points.Add(child);
                    }
                    spawnPoints = points.ToArray();
                }
                else
                {
                    // Create default spawn points
                    spawnPoints = new Transform[4];
                    GameObject pointsParent = new GameObject("SpawnPoints");
                    
                    for (int i = 0; i < 4; i++)
                    {
                        GameObject point = new GameObject("SpawnPoint_" + i);
                        point.transform.parent = pointsParent.transform;
                        
                        // Position in corners
                        float x = (i % 2 == 0) ? -8f : 8f;
                        float y = (i < 2) ? -4f : 4f;
                        point.transform.position = new Vector3(x, y, 0);
                        
                        spawnPoints[i] = point.transform;
                    }
                }
            }
            
            // Begin spawning enemies for current level
            SpawnEnemiesForLevel(currentLevel);
        }
        
        private void Update()
        {
            // Update remaining enemies if any died
            int count = activeEnemies.Count;
            activeEnemies.RemoveAll(e => e == null);
            
            if (count != activeEnemies.Count)
            {
                remainingEnemies = activeEnemies.Count;
                if (gameModel != null)
                {
                    gameModel.SetEnemiesRemaining(remainingEnemies);
                }
                
                if (remainingEnemies <= 0)
                {
                    // Level complete
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.CompleteLevel();
                    }
                }
            }
        }
        
        public void SpawnEnemiesForLevel(int level)
        {
            // Clear any existing enemies
            ClearAllEnemies();
            
            // Calculate number of enemies based on level
            int enemyCount = CalculateEnemyCount(level);
            remainingEnemies = enemyCount;
            
            if (gameModel != null)
            {
                gameModel.SetEnemiesRemaining(remainingEnemies);
            }
            
            // Start spawning
            StartCoroutine(SpawnEnemiesCoroutine(enemyCount, level));
        }
        
        private System.Collections.IEnumerator SpawnEnemiesCoroutine(int count, int level)
        {
            for (int i = 0; i < count; i++)
            {
                // Random spawn point
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                
                // Random enemy prefab
                GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
                
                // Spawn enemy
                GameObject enemy = EnemyFactory.CreateEnemy(prefab, spawnPoint.position, level);
                
                // Scale enemy stats based on level
                ScaleEnemyStats(enemy, level);
                
                // Add to active enemies
                activeEnemies.Add(enemy);
                
                // Wait before spawning next
                yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            }
        }
        
        private void ScaleEnemyStats(GameObject enemy, int level)
        {
            if (level <= 1) return;
            
            EnemyModel model = enemy.GetComponent<EnemyModel>();
            if (model != null)
            {
                // Scale health based on level
                model.stats.maxHealth = (int)(model.stats.maxHealth * Mathf.Pow(healthMultiplierPerLevel, level - 1));
                model.stats.currentHealth = model.stats.maxHealth;
                
                // Scale damage based on level
                model.stats.damage = (int)(model.stats.damage * Mathf.Pow(damageMultiplierPerLevel, level - 1));
                
                // Scale movement speed
                model.stats.moveSpeed *= Mathf.Pow(speedMultiplierPerLevel, level - 1);
                
                // Scale experience and gold value
                model.stats.experienceValue = (int)(model.stats.experienceValue * (1 + 0.15f * (level - 1)));
                model.stats.goldValue = (int)(model.stats.goldValue * (1 + 0.15f * (level - 1)));
            }
        }
        
        private int CalculateEnemyCount(int level)
        {
            return (int)(baseEnemiesPerLevel * Mathf.Pow(enemiesPerLevelMultiplier, level - 1));
        }
        
        private void ClearAllEnemies()
        {
            foreach (GameObject enemy in activeEnemies)
            {
                if (enemy != null)
                {
                    Destroy(enemy);
                }
            }
            
            activeEnemies.Clear();
        }
        
        private void HandleLevelChanged(int newLevel)
        {
            currentLevel = newLevel;
            SpawnEnemiesForLevel(currentLevel);
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            if (gameModel != null)
            {
                gameModel.OnLevelChanged -= HandleLevelChanged;
            }
        }
    }
}