using UnityEngine;

namespace ArcheroClone.Model
{
    public class ProjectileModel : MonoBehaviour
    {
        public int damage = 10;
        public float speed = 10f;
        public float lifetime = 5f;
        public bool isEnemyProjectile = false;
        
        private void Start()
        {
            Destroy(gameObject, lifetime);
        }
    }
}