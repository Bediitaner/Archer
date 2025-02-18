using ArcheroClone.Factory;
using ArcheroClone.Model;
using UnityEngine;

namespace ArcheroClone.Managers
{
    public class CollectibleManager : MonoBehaviour
    {
        public static CollectibleManager Instance { get; private set; }
        
        [Header("Collectible Prefabs")]
        public GameObject goldPrefab;
        public GameObject healthPotionPrefab;
        public GameObject experienceOrbPrefab;
        
        [Header("Drop Rates")]
        [Range(0f, 1f)]
        public float goldDropRate = 0.6f;
        [Range(0f, 1f)]
        public float healthPotionDropRate = 0.15f;
        [Range(0f, 1f)]
        public float experienceOrbDropRate = 0.4f;
        
        [Header("Value Settings")]
        public int baseGoldValue = 5;
        public int baseHealthValue = 20;
        public int baseExpValue = 10;
        public float valueMultiplierPerLevel = 1.1f;
        
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
            
            // Find game model
            gameModel = GameModel.Instance;
        }
        
        public void SpawnCollectiblesAtPosition(Vector3 position, int enemyLevel = 1)
        {
            // Determine whether to drop gold
            if (Random.value <= goldDropRate && goldPrefab != null)
            {
                int goldValue = CalculateValueForLevel(baseGoldValue, enemyLevel);
                CollectibleFactory.CreateGold(goldPrefab, position, goldValue);
            }
            
            // Determine whether to drop health potion
            if (Random.value <= healthPotionDropRate && healthPotionPrefab != null)
            {
                int healthValue = CalculateValueForLevel(baseHealthValue, enemyLevel);
                CollectibleFactory.CreateHealthPotion(healthPotionPrefab, position, healthValue);
            }
            
            // Determine whether to drop experience orb
            if (Random.value <= experienceOrbDropRate && experienceOrbPrefab != null)
            {
                int expValue = CalculateValueForLevel(baseExpValue, enemyLevel);
                CollectibleFactory.CreateExperienceOrb(experienceOrbPrefab, position, expValue);
            }
        }
        
        public void SpawnSpecificCollectible(string type, Vector3 position, int value = 0)
        {
            switch (type.ToLower())
            {
                case "gold":
                    if (goldPrefab != null)
                    {
                        int goldValue = value > 0 ? value : baseGoldValue;
                        CollectibleFactory.CreateGold(goldPrefab, position, goldValue);
                    }
                    break;
                
                case "health":
                case "healthpotion":
                    if (healthPotionPrefab != null)
                    {
                        int healthValue = value > 0 ? value : baseHealthValue;
                        CollectibleFactory.CreateHealthPotion(healthPotionPrefab, position, healthValue);
                    }
                    break;
                
                case "experience":
                case "exp":
                    if (experienceOrbPrefab != null)
                    {
                        int expValue = value > 0 ? value : baseExpValue;
                        CollectibleFactory.CreateExperienceOrb(experienceOrbPrefab, position, expValue);
                    }
                    break;
            }
        }
        
        private int CalculateValueForLevel(int baseValue, int level)
        {
            if (level <= 1) return baseValue;
            
            return (int)(baseValue * Mathf.Pow(valueMultiplierPerLevel, level - 1));
        }
    }
}