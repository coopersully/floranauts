using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interfaces
{
    public class PauseManager : MonoBehaviour
    {
        public static PauseManager Instance;
        public bool isPaused;
        
        public GameObject hud;
        
        public GameObject overlay;
        public GameObject mainPanel;

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        public void Resume()
        {
            isPaused = false;
            Time.timeScale = 1.0f;
            
            if (hud != null) hud.SetActive(true);
            
            overlay.SetActive(false);
            mainPanel.SetActive(false);
        }
        
        public void Pause()
        {
            isPaused = true;
            Time.timeScale = 0.0f;

            if (hud != null) hud.SetActive(false);
            
            overlay.SetActive(true);
            mainPanel.SetActive(true);
        }

        public void MainMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
        
    }
}
