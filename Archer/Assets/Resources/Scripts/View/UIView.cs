using ArcheroClone.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArcheroClone.View
{
    public class UIView : MonoBehaviour
    {
        [Header("HUD Elements")]
        public Slider healthBar;
        public Slider experienceBar;
        public TextMeshProUGUI levelText;
        public TextMeshProUGUI goldText;
        public TextMeshProUGUI enemiesRemainingText;
        public TextMeshProUGUI timerText;
        
        [Header("Panels")]
        public GameObject pausePanel;
        public GameObject gameOverPanel;
        public GameObject levelUpPanel;
        public GameObject victoryPanel;
        
        [Header("Level Up Options")]
        public Button[] skillButtons;
        public TextMeshProUGUI[] skillDescriptions;
        
        [Header("Game Over")]
        public TextMeshProUGUI finalScoreText;
        public TextMeshProUGUI totalEnemiesKilledText;
        public TextMeshProUGUI totalGoldCollectedText;
        public TextMeshProUGUI totalTimeText;
        
        private PlayerModel playerModel;
        private GameModel gameModel;
        
        private void Start()
        {
            // Find models
            playerModel = FindObjectOfType<PlayerModel>();
            gameModel = GameModel.Instance;
            
            // Subscribe to events
            if (playerModel != null)
            {
                playerModel.OnHealthChanged += UpdateHealthBar;
                playerModel.OnExperienceGained += UpdateExperienceBar;
                playerModel.OnLevelUp += ShowLevelUpPanel;
                playerModel.OnGoldChanged += UpdateGoldText;
                playerModel.OnPlayerDied += ShowGameOverPanel;
            }
            
            if (gameModel != null)
            {
                gameModel.OnEnemiesRemainingChanged += UpdateEnemiesRemainingText;
                gameModel.OnGameTimeUpdated += UpdateTimer;
                gameModel.OnGamePaused += ShowPausePanel;
                gameModel.OnGameResumed += HidePausePanel;
                gameModel.OnGameOver += ShowGameOverPanel;
            }
            
            // Initialize UI
            if (pausePanel != null) pausePanel.SetActive(false);
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
            if (levelUpPanel != null) levelUpPanel.SetActive(false);
            if (victoryPanel != null) victoryPanel.SetActive(false);
        }
        
        private void UpdateHealthBar(int currentHealth, int maxHealth)
        {
            if (healthBar != null)
            {
                healthBar.maxValue = maxHealth;
                healthBar.value = currentHealth;
            }
        }
        
        private void UpdateExperienceBar(int experienceGained)
        {
            if (experienceBar != null && playerModel != null)
            {
                experienceBar.maxValue = playerModel.stats.experienceToNextLevel;
                experienceBar.value = playerModel.stats.experience;
            }
        }
        
        private void UpdateGoldText(int goldAmount)
        {
            if (goldText != null)
            {
                goldText.text = goldAmount.ToString();
            }
        }
        
        private void UpdateEnemiesRemainingText(int enemiesRemaining)
        {
            if (enemiesRemainingText != null)
            {
                enemiesRemainingText.text = "Enemies: " + enemiesRemaining;
            }
        }
        
        private void UpdateTimer(float gameTime)
        {
            if (timerText != null)
            {
                int minutes = (int)(gameTime / 60);
                int seconds = (int)(gameTime % 60);
                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
        }
        
        private void ShowLevelUpPanel(int newLevel)
        {
            if (levelUpPanel != null)
            {
                if (levelText != null)
                {
                    levelText.text = "Level: " + newLevel;
                }
                
                // Generate random skill options
                GenerateSkillOptions();
                
                Time.timeScale = 0f;  // Pause the game during level up
                levelUpPanel.SetActive(true);
            }
        }
        
        private void GenerateSkillOptions()
        {
            // Generate 3 random skill options
            string[] skillTypes = { "Damage", "Health", "Speed", "Attack Speed", "Projectile Speed" };
            int[] skillValues = { 5, 20, 10, 15, 10 };
            
            for (int i = 0; i < skillButtons.Length && i < skillDescriptions.Length; i++)
            {
                int randomSkill = Random.Range(0, skillTypes.Length);
                skillDescriptions[i].text = "Increase " + skillTypes[randomSkill] + " by " + skillValues[randomSkill] + "%";
                
                // Store skill type and value in button's tag
                skillButtons[i].tag = randomSkill.ToString();
            }
        }
        
        public void OnSkillSelected(int buttonIndex)
        {
            if (buttonIndex < 0 || buttonIndex >= skillButtons.Length) return;
            
            Button selectedButton = skillButtons[buttonIndex];
            int skillType = int.Parse(selectedButton.tag);
            
            // Apply the selected skill upgrade
            ApplySkillUpgrade(skillType);
            
            // Hide the level up panel and resume game
            if (levelUpPanel != null)
            {
                levelUpPanel.SetActive(false);
                Time.timeScale = 1f;
            }
        }
        
        private void ApplySkillUpgrade(int skillType)
        {
            if (playerModel == null) return;
            
            switch (skillType)
            {
                case 0: // Damage
                    playerModel.stats.damage = (int)(playerModel.stats.damage * 1.05f);
                    break;
                case 1: // Health
                    int healthIncrease = (int)(playerModel.stats.maxHealth * 0.2f);
                    playerModel.stats.maxHealth += healthIncrease;
                    playerModel.stats.currentHealth += healthIncrease;
                    playerModel.OnHealthChanged?.Invoke(playerModel.stats.currentHealth, playerModel.stats.maxHealth);
                    break;
                case 2: // Speed
                    playerModel.stats.moveSpeed *= 1.1f;
                    break;
                case 3: // Attack Speed
                    playerModel.stats.attackSpeed *= 1.15f;
                    break;
                case 4: // Projectile Speed
                    playerModel.stats.projectileSpeed *= 1.1f;
                    break;
            }
        }
        
        private void ShowPausePanel()
        {
            if (pausePanel != null)
            {
                pausePanel.SetActive(true);
            }
        }
        
        private void HidePausePanel()
        {
            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }
        }
        
        private void ShowGameOverPanel()
        {
            if (gameOverPanel != null)
            {
                UpdateFinalStats();
                gameOverPanel.SetActive(true);
            }
        }
        
        private void UpdateFinalStats()
        {
            if (gameModel == null) return;
            
            if (finalScoreText != null)
            {
                int finalScore = gameModel.stats.totalEnemiesKilled * 100 + gameModel.stats.totalGoldCollected;
                finalScoreText.text = "Final Score: " + finalScore;
            }
            
            if (totalEnemiesKilledText != null)
            {
                totalEnemiesKilledText.text = "Enemies Killed: " + gameModel.stats.totalEnemiesKilled;
            }
            
            if (totalGoldCollectedText != null)
            {
                totalGoldCollectedText.text = "Gold Collected: " + gameModel.stats.totalGoldCollected;
            }
            
            if (totalTimeText != null)
            {
                int minutes = (int)(gameModel.stats.gameTime / 60);
                int seconds = (int)(gameModel.stats.gameTime % 60);
                totalTimeText.text = "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds);
            }
        }
        
        public void OnResumeClicked()
        {
            if (gameModel != null)
            {
                gameModel.ResumeGame();
            }
        }
        
        public void OnRestartClicked()
        {
            if (gameModel != null)
            {
                gameModel.ResetGame();
                UnityEngine.SceneManagement.SceneManager.LoadScene(0); // Load the first scene
            }
        }
        
        public void OnQuitClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}