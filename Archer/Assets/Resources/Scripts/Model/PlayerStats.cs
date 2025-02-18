// PlayerModel.cs

namespace ArcheroClone.Model
{
    [System.Serializable]
    public class PlayerStats
    {
        public int maxHealth = 100;
        public int currentHealth;
        public float moveSpeed = 5f;
        public float attackSpeed = 1f;
        public int damage = 10;
        public float projectileSpeed = 10f;
        public int goldCoins = 0;
        public int level = 1;
        public int experience = 0;
        public int experienceToNextLevel = 100;
    }
}