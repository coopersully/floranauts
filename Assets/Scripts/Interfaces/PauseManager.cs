using Audio;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

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
            if (restrictions.IsRestricted() || LoadingScreen.IsLoading)
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
        public void ControlsBack()
        {
            mainPanel.SetActive(true);
            controlsPanel.SetActive(false);
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
            
            // Unlock cursors
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            // Load main menu scene
            LoadingScreen.Instance.Load(0);
        }

        // "Exit Game" button
        public void ExitGame()
        {
            Application.Quit();
        }

        
        
    }
}
