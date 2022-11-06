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

        public int score;

        public List<CapturePoint> inventory;
        public CapturePoint currentCapturePoint;
        
        public Color playerColor;
        public SkinnedMeshRenderer playerSkin;
        public SkinnedMeshRenderer playerAccents;
        
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
            
            // Colorize the player model
            playerSkin.material.color = playerColor;
            playerAccents.material.color = playerColor;
            
            // Start incrementing the player's score every second
            StartCoroutine(IncrementScore());
        }
        
        // Called every time a player presses the 'Capture' binding.
        public void Capture(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            currentCapturePoint.CaptureAction(this);
        }

        /* When a player enters a 'CapturePoint' on a planet, set it
         to their currently selected CapturePoint in the script so that
         if they press any buttons (above) it will call the function on
         that CapturePoint. */
        private void OnTriggerEnter(Collider other)
        {
            // If the entered trigger is not a capture point, ignore it.
            if (!other.CompareTag("CapturePoint")) return;

            currentCapturePoint = other.GetComponent<CapturePoint>();
        }
        
        /* When a player exits a 'CapturePoint' on a
         planet, nullify their current capture point. */
        private void OnTriggerExit(Collider other)
        {
            // If the entered trigger is not a capture point, ignore it.
            if (!other.CompareTag("CapturePoint")) return;

            currentCapturePoint = null;
        }
        
        /* Called once every second. Increments the player's
         score by the amount of Capture Points they currently own. */
        private IEnumerator IncrementScore()
        {
            while (true)
            {
                score += inventory.Count;
                Debug.Log(name + " has a score of " + score);
                yield return new WaitForSeconds(1.0f);
            }
        }
    }
}
