using ArcheroClone.Controller;
using UnityEngine;

namespace ArcheroClone.Factory
{
    public class CollectibleFactory : MonoBehaviour
    {
        public static GameObject CreateGold(GameObject goldPrefab, Vector3 position, int amount = 5)
        {
            GameObject gold = Instantiate(goldPrefab, position, Quaternion.identity);
            
            // Ensure it has the GoldPickup component
            GoldPickup goldComponent = gold.GetComponent<GoldPickup>();
            if (goldComponent == null)
            {
                goldComponent = gold.AddComponent<GoldPickup>();
            }
            goldComponent.goldAmount = amount;
            
            // Ensure it has a collider
            if (gold.GetComponent<CircleCollider2D>() == null)
            {
                CircleCollider2D collider = gold.AddComponent<CircleCollider2D>();
                collider.isTrigger = true;
            }
            
            // Add slight random offset to avoid collectibles stacking
            gold.transform.position += new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
            
            // Tag the gold
            gold.tag = "Gold";
            
            return gold;
        }
        
        public static GameObject CreateHealthPotion(GameObject potionPrefab, Vector3 position, int healAmount = 20)
        {
            GameObject potion = Instantiate(potionPrefab, position, Quaternion.identity);
            
            // Ensure it has the HealthPotion component
            HealthPotion potionComponent = potion.GetComponent<HealthPotion>();
            if (potionComponent == null)
            {
                potionComponent = potion.AddComponent<HealthPotion>();
            }
            potionComponent.healAmount = healAmount;
            
            // Ensure it has a collider
            if (potion.GetComponent<CircleCollider2D>() == null)
            {
                CircleCollider2D collider = potion.AddComponent<CircleCollider2D>();
                collider.isTrigger = true;
            }
            
            // Add slight random offset to avoid collectibles stacking
            potion.transform.position += new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
            
            // Tag the potion
            potion.tag = "HealthPotion";
            
            return potion;
        }
        
        public static GameObject CreateExperienceOrb(GameObject orbPrefab, Vector3 position, int expAmount = 10)
        {
            GameObject orb = Instantiate(orbPrefab, position, Quaternion.identity);
            
            // Ensure it has the ExperienceOrb component
            ExperienceOrb orbComponent = orb.GetComponent<ExperienceOrb>();
            if (orbComponent == null)
            {
                orbComponent = orb.AddComponent<ExperienceOrb>();
            }
            orbComponent.experienceAmount = expAmount;
            
            // Ensure it has a collider
            if (orb.GetComponent<CircleCollider2D>() == null)
            {
                CircleCollider2D collider = orb.AddComponent<CircleCollider2D>();
                collider.isTrigger = true;
            }
            
            // Add slight random offset to avoid collectibles stacking
            orb.transform.position += new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
            
            // Tag the orb
            orb.tag = "ExperienceOrb";
            
            return orb;
        }
    }
}