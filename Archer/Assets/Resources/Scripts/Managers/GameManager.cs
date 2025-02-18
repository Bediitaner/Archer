using System.Collections.Generic;
using ArcheroClone.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ArcheroClone.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        [Header("Level Settings")]
        public int currentLevel = 1;
        public int maxLevel = 10;
        public float levelCompletionTimer = 5f;
        
        [Header("Difficulty Settings")]
        public float difficultyMultiplier = 1.2f;
        
        [Header("References")]
        public GameObject playerPrefab;
        public List<GameObject> enemyPrefabs;
        public GameObject uiPrefab;
        
        private GameModel gameModel;
        private float levelCompletionCountdown;
        private bool isLevelCompleted = false;
        
        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            // Find or create game model
            gameModel = FindObjectOfType<GameModel>();
            if (gameModel == null && GameModel.Instance == null)
            {
                GameObject gameModelObj = new GameObject("GameModel");
                gameModel = gameModelObj.AddComponent<GameModel>();
            }
            else if (GameModel.Instance != null)
            {
                gameModel = GameModel.Instance;
            }
        }
        
        private void Start()
        {
            // Subscribe to events
            if (gameModel != null)
            {
                gameModel.OnGameOver += HandleGameOver;
                gameModel.OnLevelChanged += HandleLevelChanged;
            }
        }
        
        private void Update()
        {
            // Check for level completion
            if (isLevelCompleted)
            {
                levelCompletionCountdown -= Time.deltaTime;
                if (levelCompletionCountdown <= 0f)
                {
                    LoadNextLevel();
                }
            }
            
            // Handle pause input
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }
        
        public void StartNewGame()
        {
            if (gameModel != null)
            {
                gameModel.ResetGame();
                currentLevel = 1;
                LoadLevel(currentLevel);
            }
        }
        
        public void LoadLevel(int level)
        {
            // Ensure level is valid
            level = Mathf.Clamp(level, 1, maxLevel);
            currentLevel = level;
            
            // Load the appropriate scene (assuming scene names match level numbers)
            string sceneName = "Level_" + level;
            
            // Check if scene exists
            if (Application.CanStreamedLevelBeLoaded(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                // Fallback to first level if the requested level doesn't exist
                SceneManager.LoadScene("Level_1");
            }
            
            isLevelCompleted = false;
        }
        
        public void LoadNextLevel()
        {
            if (currentLevel < maxLevel)
            {
                LoadLevel(currentLevel + 1);
            }
            else
            {
                // Win game
                if (gameModel != null)
                {
                    // gameModel.WinGame(); // Implement this if needed
                }
                
                // For now, just load the first level again
                LoadLevel(1);
            }
        }
        
        public void CompleteLevel()
        {
            isLevelCompleted = true;
            levelCompletionCountdown = levelCompletionTimer;
            
            if (gameModel != null)
            {
                gameModel.AdvanceToNextLevel();
            }
        }
        
        public void TogglePause()
        {
            if (gameModel != null)
            {
                if (gameModel.stats.isPaused)
                {
                    gameModel.ResumeGame();
                }
                else
                {
                    gameModel.PauseGame();
                }
            }
        }
        
        private void HandleGameOver()
        {
            // Additional game over logic if needed
            Debug.Log("Game Over");
        }
        
        private void HandleLevelChanged(int newLevel)
        {
            currentLevel = newLevel;
            // Additional level change logic if needed
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            if (gameModel != null)
            {
                gameModel.OnGameOver -= HandleGameOver;
                gameModel.OnLevelChanged -= HandleLevelChanged;
            }
        }
    }
}