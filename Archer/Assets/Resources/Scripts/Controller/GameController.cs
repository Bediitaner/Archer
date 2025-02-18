using ArcheroClone.Model;
using UnityEngine;

namespace ArcheroClone.Controller
{
    public class GameController : MonoBehaviour
    {
        private GameModel gameModel;
        
        private void Awake()
        {
            gameModel = GameModel.Instance;
        }
        
        private void Update()
        {
            // Handle pause input
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }
        
        private void TogglePause()
        {
            if (gameModel == null) return;
            
            if (gameModel.stats.isPaused)
            {
                gameModel.ResumeGame();
            }
            else
            {
                gameModel.PauseGame();
            }
        }
    }
}