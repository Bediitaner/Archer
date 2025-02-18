using UnityEngine;

namespace ArcheroClone.Model
{
    public class GameModel : MonoBehaviour
    {
        public static GameModel Instance { get; private set; }

        public GameStats stats;
        public System.Action<int> OnLevelChanged;
        public System.Action<int> OnEnemiesRemainingChanged;
        public System.Action OnGameOver;
        public System.Action OnGamePaused;
        public System.Action OnGameResumed;
        public System.Action<float> OnGameTimeUpdated;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (!stats.isPaused && !stats.isGameOver)
            {
                stats.gameTime += Time.deltaTime;
                OnGameTimeUpdated?.Invoke(stats.gameTime);
            }
        }

        public void SetEnemiesRemaining(int count)
        {
            stats.enemiesRemaining = count;
            OnEnemiesRemainingChanged?.Invoke(count);
        }

        public void EnemyKilled()
        {
            stats.enemiesRemaining--;
            stats.totalEnemiesKilled++;
            OnEnemiesRemainingChanged?.Invoke(stats.enemiesRemaining);
            
            if (stats.enemiesRemaining <= 0)
            {
                // All enemies defeated, next level
                AdvanceToNextLevel();
            }
        }

        public void AddGoldCollected(int amount)
        {
            stats.totalGoldCollected += amount;
        }

        public void AdvanceToNextLevel()
        {
            stats.currentLevel++;
            OnLevelChanged?.Invoke(stats.currentLevel);
        }

        public void PauseGame()
        {
            stats.isPaused = true;
            Time.timeScale = 0f;
            OnGamePaused?.Invoke();
        }

        public void ResumeGame()
        {
            stats.isPaused = false;
            Time.timeScale = 1f;
            OnGameResumed?.Invoke();
        }

        public void GameOver()
        {
            stats.isGameOver = true;
            OnGameOver?.Invoke();
        }

        public void ResetGame()
        {
            stats.currentLevel = 1;
            stats.isGameOver = false;
            stats.isPaused = false;
            stats.gameTime = 0f;
            stats.totalGoldCollected = 0;
            stats.totalEnemiesKilled = 0;
            Time.timeScale = 1f;
        }
    }
}