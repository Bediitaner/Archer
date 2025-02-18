namespace ArcheroClone.Model
{
    [System.Serializable]
    public class GameStats
    {
        public int currentLevel = 1;
        public int enemiesRemaining;
        public bool isGameOver = false;
        public bool isPaused = false;
        public float gameTime = 0f;
        public int totalGoldCollected = 0;
        public int totalEnemiesKilled = 0;
    }
}