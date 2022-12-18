using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerControlsMisc : MonoBehaviour
    {
        public bool controlPanelUp = false;
        public void PauseButtonPressed(InputAction.CallbackContext context)
        {
            if (!context.started) return;

            if (PauseManager.Instance.isPaused && !controlPanelUp) PauseManager.Instance.Resume();
            else PauseManager.Instance.Pause();
        }
        public void ResumeButton(InputAction.CallbackContext context)
        {
            if (PauseManager.Instance.isPaused && context.started && !controlPanelUp)
                PauseManager.Instance.Resume();
            else if (!PauseManager.Instance.isPaused && context.started && !controlPanelUp)
                PauseManager.Instance.Pause();
            
        }
        public void ControlsButton(InputAction.CallbackContext context)
        {
            if (PauseManager.Instance.isPaused && context.started && !controlPanelUp)
            {
                PauseManager.Instance.Controls();
                controlPanelUp = true;
            }

            else if (PauseManager.Instance.isPaused && context.started && controlPanelUp)
            {
                PauseManager.Instance.ControlsBack();
                controlPanelUp = false;
            }

        }
        public void MenuButton(InputAction.CallbackContext context)
        {
            if (PauseManager.Instance.isPaused && context.started && !controlPanelUp)
                PauseManager.Instance.MainMenu();
        }
        public void ExitButton(InputAction.CallbackContext context)
        {
            if (PauseManager.Instance.isPaused && context.started && !controlPanelUp)
                PauseManager.Instance.ExitGame();
        }

    }
}
