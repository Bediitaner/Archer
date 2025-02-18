using ArcheroClone.Model;
using UnityEngine;

namespace ArcheroClone.Controller
{
    public class DoorController : MonoBehaviour
    {
        private bool isDoorOpen = false;
        
        private void Update()
        {
            // Open door when all enemies are defeated
            if (!isDoorOpen && GameModel.Instance != null && GameModel.Instance.stats.enemiesRemaining <= 0)
            {
                OpenDoor();
            }
        }
        
        private void OpenDoor()
        {
            isDoorOpen = true;
            
            // Change door appearance (could animate, change sprite, etc.)
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.green;
            }
            
            // Add glow effect or particle system if desired
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isDoorOpen && other.CompareTag("Player"))
            {
                // Player entered the open door, advance to next level
                if (GameModel.Instance != null)
                {
                    GameModel.Instance.AdvanceToNextLevel();
                }
            }
        }
    }
}