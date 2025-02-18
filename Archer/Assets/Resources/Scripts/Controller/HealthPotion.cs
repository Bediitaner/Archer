using UnityEngine;

namespace ArcheroClone.Controller
{
    public class HealthPotion : MonoBehaviour
    {
        public int healAmount = 20;
        public float lifetime = 20f;
        
        private void Start()
        {
            Destroy(gameObject, lifetime);
        }
    }
}