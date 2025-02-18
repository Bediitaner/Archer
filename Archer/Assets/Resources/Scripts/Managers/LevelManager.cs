using System.Collections.Generic;
using ArcheroClone.Factory;
using ArcheroClone.Model;
using UnityEngine;

namespace ArcheroClone.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }
        
        [Header("Level Generation")]
        public GameObject floorPrefab;
        public GameObject wallPrefab;
        public GameObject doorPrefab;
        public int roomWidth = 20;
        public int roomHeight = 12;
        
        [Header("Level Objects")]
        public List<GameObject> obstaclesPrefabs;
        public int minObstacles = 3;
        public int maxObstacles = 8;
        
        private Transform levelContainer;
        private GameModel gameModel;
        
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
            
            // Create a container for level objects
            GameObject containerObj = new GameObject("LevelContainer");
            levelContainer = containerObj.transform;
            
            // Find game model
            gameModel = GameModel.Instance;
        }
        
        public void GenerateRoom()
        {
            ClearCurrentRoom();
            CreateFloor();
            CreateWalls();
            PlaceObstacles();
        }
        
        private void CreateFloor()
        {
            if (floorPrefab == null) return;
            
            // Create floor tiles
            GameObject floorParent = new GameObject("Floor");
            floorParent.transform.parent = levelContainer;
            
            for (int x = -roomWidth/2; x < roomWidth/2; x++)
            {
                for (int y = -roomHeight/2; y < roomHeight/2; y++)
                {
                    LevelObjectFactory.CreateFloor(floorPrefab, new Vector3(x, y, 0), floorParent.transform);
                }
            }
        }
        
        private void CreateWalls()
        {
            if (wallPrefab == null) return;
            
            // Create wall parent
            GameObject wallParent = new GameObject("Walls");
            wallParent.transform.parent = levelContainer;
            
            // Create horizontal walls
            for (int x = -roomWidth/2 - 1; x <= roomWidth/2; x++)
            {
                // Bottom wall
                LevelObjectFactory.CreateWall(wallPrefab, new Vector3(x, -roomHeight/2 - 1, 0), wallParent.transform);
                
                // Top wall
                LevelObjectFactory.CreateWall(wallPrefab, new Vector3(x, roomHeight/2, 0), wallParent.transform);
            }
            
            // Create vertical walls
            for (int y = -roomHeight/2; y < roomHeight/2; y++)
            {
                // Left wall
                LevelObjectFactory.CreateWall(wallPrefab, new Vector3(-roomWidth/2 - 1, y, 0), wallParent.transform);
                
                // Right wall
                LevelObjectFactory.CreateWall(wallPrefab, new Vector3(roomWidth/2, y, 0), wallParent.transform);
            }
            
            // Create door if prefab exists
            if (doorPrefab != null)
            {
                LevelObjectFactory.CreateDoor(doorPrefab, new Vector3(0, roomHeight/2, 0), wallParent.transform);
            }
        }
        
        private void PlaceObstacles()
        {
            if (obstaclesPrefabs == null || obstaclesPrefabs.Count == 0) return;
            
            GameObject obstaclesParent = new GameObject("Obstacles");
            obstaclesParent.transform.parent = levelContainer;
            
            int obstacleCount = Random.Range(minObstacles, maxObstacles + 1);
            
            for (int i = 0; i < obstacleCount; i++)
            {
                // Random position within room bounds, away from center and edges
                float x = Random.Range(-roomWidth/2 + 2, roomWidth/2 - 1);
                float y = Random.Range(-roomHeight/2 + 2, roomHeight/2 - 1);
                
                // Avoid center area where player spawns
                if (Mathf.Abs(x) < 3 && Mathf.Abs(y) < 3)
                {
                    // Adjust position further from center
                    if (x > 0) x += 3;
                    else x -= 3;
                }
                
                Vector3 position = new Vector3(x, y, 0);
                
                // Random obstacle prefab
                GameObject prefab = obstaclesPrefabs[Random.Range(0, obstaclesPrefabs.Count)];
                
                // Create obstacle
                GameObject obstacle = Instantiate(prefab, position, Quaternion.identity, obstaclesParent.transform);
                
                // Add collider if needed
                if (obstacle.GetComponent<Collider2D>() == null)
                {
                    BoxCollider2D collider = obstacle.AddComponent<BoxCollider2D>();
                    collider.isTrigger = false;
                }
            }
        }
        
        private void ClearCurrentRoom()
        {
            // Destroy all children of level container
            if (levelContainer != null)
            {
                foreach (Transform child in levelContainer)
                {
                    Destroy(child.gameObject);
                }
            }
        }
        
        public Vector3 GetRandomPositionInRoom()
        {
            float x = Random.Range(-roomWidth/2 + 2, roomWidth/2 - 2);
            float y = Random.Range(-roomHeight/2 + 2, roomHeight/2 - 2);
            return new Vector3(x, y, 0);
        }
        
        public bool IsWithinRoomBounds(Vector3 position)
        {
            return position.x > -roomWidth/2 && position.x < roomWidth/2 &&
                   position.y > -roomHeight/2 && position.y < roomHeight/2;
        }
    }
}