using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerControlsMisc : MonoBehaviour
    {
        public void PauseButtonPressed(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            
            if (PauseManager.Instance.isPaused)
            {
                Cursor.lockState = CursorLockMode.Locked;
                PauseManager.Instance.Resume();
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                PauseManager.Instance.Pause();
            }
        }
    }
}
