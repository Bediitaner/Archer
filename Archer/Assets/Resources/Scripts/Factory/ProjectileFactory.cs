using ArcheroClone.Controller;
using ArcheroClone.Model;
using ArcheroClone.View;
using UnityEngine;

namespace ArcheroClone.Factory
{
    public class ProjectileFactory : MonoBehaviour
    {
        public static GameObject CreateProjectile(GameObject projectilePrefab, Vector3 position, Vector2 direction, float speed, int damage, bool isEnemyProjectile = false)
        {
            GameObject projectile = Instantiate(projectilePrefab, position, Quaternion.identity);
            
            // Ensure the projectile has all required components
            if (projectile.GetComponent<ProjectileModel>() == null)
            {
                ProjectileModel model = projectile.AddComponent<ProjectileModel>();
                model.damage = damage;
                model.speed = speed;
                model.isEnemyProjectile = isEnemyProjectile;
            }
            else
            {
                ProjectileModel model = projectile.GetComponent<ProjectileModel>();
                model.damage = damage;
                model.speed = speed;
                model.isEnemyProjectile = isEnemyProjectile;
            }
                
            if (projectile.GetComponent<ProjectileView>() == null)
                projectile.AddComponent<ProjectileView>();
                
            if (projectile.GetComponent<ProjectileController>() == null)
                projectile.AddComponent<ProjectileController>();
                
            if (projectile.GetComponent<Rigidbody2D>() == null)
            {
                Rigidbody2D rb = projectile.AddComponent<Rigidbody2D>();
                rb.gravityScale = 0f;
                rb.velocity = direction.normalized * speed;
            }
            else
            {
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                rb.velocity = direction.normalized * speed;
            }
            
            if (projectile.GetComponent<CircleCollider2D>() == null)
            {
                CircleCollider2D collider = projectile.AddComponent<CircleCollider2D>();
                collider.isTrigger = true;
            }
            
            // Tag the projectile based on who fired it
            projectile.tag = isEnemyProjectile ? "EnemyProjectile" : "PlayerProjectile";
            
            // Rotate projectile to face direction of movement
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            return projectile;
        }
    }
}