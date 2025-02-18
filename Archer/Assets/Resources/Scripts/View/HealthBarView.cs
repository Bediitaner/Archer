using UnityEngine;
using UnityEngine.UI;

namespace ArcheroClone.View
{
    public class HealthBarView : MonoBehaviour
    {
        public Image fillImage;
        
        public void UpdateHealthBar(float fillAmount)
        {
            if (fillImage != null)
            {
                fillImage.fillAmount = fillAmount;
                
                // Change color based on health percentage
                if (fillAmount <= 0.3f)
                    fillImage.color = Color.red;
                else if (fillAmount <= 0.6f)
                    fillImage.color = Color.yellow;
                else
                    fillImage.color = Color.green;
            }
        }
    }
}