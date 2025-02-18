using UnityEngine;

namespace ArcheroClone.Controller
{
    public class GoldPickup : MonoBehaviour
    {
        public int goldAmount = 5;
        public float lifetime = 20f;
        
        private void Start()
        {
            Destroy(gameObject, lifetime);
        }
    }
}