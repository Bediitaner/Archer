using UnityEngine;

namespace ArcheroClone.Controller
{
    public class ExperienceOrb : MonoBehaviour
    {
        public int experienceAmount = 10;
        public float lifetime = 20f;
        public float magnetDistance = 3f;
        public float moveSpeed = 5f;
        
        private Transform player;
        
        private void Start()
        {
            Destroy(gameObject, lifetime);
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
        
        private void Update()
        {
            if (player == null) return;
            
            // Check if player is in range
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance <= magnetDistance)
            {
                // Move towards player
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
                
                // Increase speed as it gets closer
                moveSpeed = 5f + (magnetDistance - distance) * 2f;
            }
        }
    }
}