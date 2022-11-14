using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerCapture : MonoBehaviour
    {
        private bool _canCapture;
        public PlayerInput playerInput;

        public int score;

        public List<CapturePoint> inventory;
        public CapturePoint currentCapturePoint;
        
        public Color primaryColor;
        public Color accentColor;
        
        public SkinnedMeshRenderer playerSkin;
        public SkinnedMeshRenderer playerAccents;
        
        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            if (playerInput.playerIndex == 0)
            {
                // primaryColor = new Color(57, 161, 243, 100); // Blue
                // accentColor = new Color(8, 235, 173, 100);   // Teal
                primaryColor = Color.cyan;
                accentColor = Color.green;
            }
            else
            {
                // primaryColor = new Color(231, 89, 78, 100); // Red
                // accentColor = new Color(251, 193, 50, 100); // Yellow
                primaryColor = Color.red;
                accentColor = Color.yellow;
            }

            // Colorize the player model
            playerSkin.material.color = primaryColor;
            playerAccents.material.color = accentColor;
            
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
            while (!ScoreManager.Instance.hasWon)
            {
                score += inventory.Count;
                if (score >= 100) GameOver.Instance.Trigger(playerInput.playerIndex);
                
                //Debug.Log(name + " has a score of " + score);
                
                yield return new WaitForSeconds(3.0f);
            }
        }
    }
}
