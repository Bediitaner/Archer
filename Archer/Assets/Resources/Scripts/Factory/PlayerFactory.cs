// PlayerFactory.cs
using UnityEngine;
using ArcheroClone.Model;
using ArcheroClone.View;
using ArcheroClone.Controller;

namespace ArcheroClone.Factory
{
    public class PlayerFactory : MonoBehaviour
    {
        public static GameObject CreatePlayer(GameObject playerPrefab, Vector3 position)
        {
            GameObject player = Instantiate(playerPrefab, position, Quaternion.identity);
            
            // Ensure the player has all required components
            if (player.GetComponent<PlayerModel>() == null)
                player.AddComponent<PlayerModel>();
                
            if (player.GetComponent<PlayerView>() == null)
                player.AddComponent<PlayerView>();
                
            if (player.GetComponent<PlayerController>() == null)
                player.AddComponent<PlayerController>();
                
            if (player.GetComponent<Rigidbody2D>() == null)
            {
                Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
                rb.gravityScale = 0f;
                rb.freezeRotation = true;
            }
            
            if (player.GetComponent<BoxCollider2D>() == null)
            {
                BoxCollider2D collider = player.AddComponent<BoxCollider2D>();
                collider.isTrigger = false;
            }
            
            if (player.GetComponent<CircleCollider2D>() == null)
            {
                CircleCollider2D collider = player.AddComponent<CircleCollider2D>();
                collider.isTrigger = true;
            }
            
            // Tag the player
            player.tag = "Player";
            
            return player;
        }
    }
}