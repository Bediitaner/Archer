using System.Collections.Generic;
using ArcheroClone.Model;
using UnityEngine;

namespace ArcheroClone.Controller
{
    public class LevelController : MonoBehaviour
    {
        public static LevelController Instance { get; private set; }
        
        [Header("Level Generation")]
        public int roomWidth = 20;
        public int roomHeight = 20;
        public GameObject wallPrefab;
        public GameObject floorPrefab;
        public GameObject doorPrefab;
        public Transform playerStartPosition;
        
        [Header("Enemies")]
        public List<GameObject> enemyPrefabs;
        public int minEnemiesPerRoom = 3;
        public int maxEnemiesPerRoom = 7;
        
        [Header("Collectibles")]
        public GameObject goldPrefab;
        public GameObject healthPotionPrefab;
        public GameObject experienceOrbPrefab;
        
        [Header("Player")]
        public GameObject playerPrefab;
        
        private List<Vector2> floorTiles = new List<Vector2>();
        private GameObject currentLevel;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            // Subscribe to game model events
            if (GameModel.Instance != null)
            {
                GameModel.Instance.OnLevelChanged += GenerateNewLevel;
            }
            
            // Generate initial level
            GenerateLevel(1);
        }
        
        private void GenerateNewLevel(int levelNumber)
        {
            // Clean up old level
            if (currentLevel != null)
            {
                Destroy(currentLevel);
            }
            
            // Generate new level
            GenerateLevel(levelNumber);
        }
        
        private void GenerateLevel(int levelNumber)
        {
            // Create a parent object for the level
            currentLevel = new GameObject("Level_" + levelNumber);
            
            // Clear floor tiles list
            floorTiles.Clear();
            
            // Generate room layout
            GenerateRoom(currentLevel.transform);
            
            // Spawn player
            SpawnPlayer();
            
            // Spawn enemies (more enemies in higher levels)
            int enemyCount = Mathf.Min(minEnemiesPerRoom + levelNumber - 1, maxEnemiesPerRoom);
            SpawnEnemies(enemyCount);
        }
        
        private void GenerateRoom(Transform parent)
        {
            // Generate walls and floor
            for (int x = 0; x < roomWidth; x++)
            {
                for (int y = 0; y < roomHeight; y++)
                {
                    GameObject tile;
                    
                    // Place walls around the edges
                    if (x == 0 || x == roomWidth - 1 || y == 0 || y == roomHeight - 1)
                    {
                        tile = Instantiate(wallPrefab, new Vector3(x, y, 0), Quaternion.identity, parent);
                        tile.name = "Wall_" + x + "_" + y;
                    }
                    else
                    {
                        tile = Instantiate(floorPrefab, new Vector3(x, y, 0), Quaternion.identity, parent);
                        tile.name = "Floor_" + x + "_" + y;
                        
                        // Add floor position to list
                        floorTiles.Add(new Vector2(x, y));
                    }
                }
            }
            
            // Add door at the top center
            int doorX = roomWidth / 2;
            GameObject door = Instantiate(doorPrefab, new Vector3(doorX, roomHeight - 1, 0), Quaternion.identity, parent);
            door.name = "Door";
            
            // Add door collision script
            DoorController doorController = door.AddComponent<DoorController>();
        }
        
        private void SpawnPlayer()
        {
            Vector3 spawnPosition;
            
            if (playerStartPosition != null)
            {
                spawnPosition = playerStartPosition.position;
            }
            else
            {
                // Default spawn position at bottom center
                spawnPosition = new Vector3(roomWidth / 2, 2, 0);
            }
            
            if (playerPrefab != null)
            {
                Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            }
        }
        
        private void SpawnEnemies(int count)
        {
            if (enemyPrefabs.Count == 0 || floorTiles.Count == 0) return;
            
            // Shuffle floor tiles to get random positions
            ShuffleList(floorTiles);
            
            // Spawn enemies
            int spawned = 0;
            int tileIndex = 0;
            
            while (spawned < count && tileIndex < floorTiles.Count)
            {
                // Skip tiles near the player spawn point
                Vector2 tilePos = floorTiles[tileIndex];
                if (Vector2.Distance(tilePos, new Vector2(roomWidth / 2, 2)) < 5)
                {
                    tileIndex++;
                    continue;
                }
                
                // Select random enemy prefab
                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
                
                // Spawn enemy
                GameObject enemy = Instantiate(enemyPrefab, new Vector3(tilePos.x, tilePos.y, 0), Quaternion.identity);
                enemy.name = "Enemy_" + spawned;
                
                spawned++;
                tileIndex++;
            }
            
            // Update enemy count in game model
            if (GameModel.Instance != null)
            {
                GameModel.Instance.SetEnemiesRemaining(spawned);
            }
        }
        
        // Utility method to shuffle a list
        private void ShuffleList<T>(List<T> list)
        {
            int n = list.Count;
            for (int i = 0; i < n; i++)
            {
                int r = i + Random.Range(0, n - i);
                T temp = list[i];
                list[i] = list[r];
                list[r] = temp;
            }
        }
    }
}