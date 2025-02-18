namespace ArcheroClone.Model
{
    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth = 50;
        public int currentHealth;
        public int damage = 10;
        public float moveSpeed = 2f;
        public float attackRate = 1f;
        public float attackRange = 1.5f;
        public int experienceValue = 10;
        public int goldValue = 5;
        public bool isRanged = false;
        public float projectileSpeed = 7f;
    }
}