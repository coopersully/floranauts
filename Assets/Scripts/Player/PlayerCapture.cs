using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerCapture : MonoBehaviour
    {
        private bool _canCapture;
        private PlayerInput _playerInput;
        
        public CapturePoint capturePoint;
        public Color playerColor;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            playerColor = _playerInput.playerIndex switch
            {
                0 => Color.yellow,
                1 => Color.green,
                3 => Color.magenta,
                4 => Color.blue,
                _ => playerColor
            };
        }

        public void Capture(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            capturePoint.CaptureAction(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            // If the entered trigger is not a capture point, ignore it.
            if (!other.CompareTag("Planet1")) return;

            capturePoint = other.GetComponent<CapturePoint>();
        }

        private void OnTriggerExit(Collider other)
        {
            // If the entered trigger is not a capture point, ignore it.
            if (!other.CompareTag("Planet1")) return;

            capturePoint = null;
        }
    }
}
