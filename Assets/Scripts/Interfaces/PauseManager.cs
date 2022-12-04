using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interfaces
{
    public class PauseManager : MonoBehaviour
    {
        public static PauseManager Instance;
        public bool isPaused;

        public Restrictions restrictions;
        
        [Header("Panels")]
        public GameObject hud;
        public GameObject mainPanel;
        public GameObject controlsPanel;
        public GameObject overlay;

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }
        
        /* PAUSES the game universally, plays an audio-cue,
         and disables the heads-up-display for all users. */
        public void Pause()
        {
            if (restrictions.IsRestricted())
            {
                Debug.Log("Pause was attempted & blocked because of restrictions.");
                AudioManager.Instance.ui.Click01();
                return;
            }
            
            isPaused = true;
            
            Time.timeScale = 0.0f;
            
            Cursor.lockState = CursorLockMode.None;
            
            SetHUDVisibility(false);
            
            ReturnToMainMenu();
        }

        /* UN-PAUSES/RESUMES the game universally, plays an audio-cue,
         and re-enables the heads-up-display for all users. */
        public void Resume()
        {
            if (restrictions.IsRestricted())
            {
                Debug.Log("Resume was attempted & blocked because of restrictions.");
                AudioManager.Instance.ui.Click01();
                return;
            }

            isPaused = false;
            
            Time.timeScale = 1.0f;
            
            Cursor.lockState = CursorLockMode.Locked;
            
            SetHUDVisibility(true);
            
            overlay.SetActive(false);
            mainPanel.SetActive(false);
        }
        
        // Enable/disable all H.U.D. elements at once
        public void SetHUDVisibility(bool isVisible)
        {
            if (hud == null) return;
            hud.SetActive(isVisible);
        }

        public void Controls()
        {
            mainPanel.SetActive(false);
            controlsPanel.SetActive(true);
        }

        public void ReturnToMainMenu()
        {
            controlsPanel.SetActive(false);
            mainPanel.SetActive(true);
            overlay.SetActive(true);
        }
        
        // "Main Menu" button
        public void MainMenu()
        {
            Resume();
            Cursor.lockState = CursorLockMode.None;
            Destroy(PlayerManager.Instance.gameObject);
            SceneManager.LoadScene(0);
        }

        // "Exit Game" button
        public void ExitGame()
        {
            Application.Quit();
        }
        
    }
}
