using UnityEngine;

namespace ArcheroClone.View
{
    public class ProjectileView : MonoBehaviour
    {
        public GameObject impactEffectPrefab;
        
        public void PlayImpactEffect()
        {
            if (impactEffectPrefab != null)
            {
                Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}