using System.Collections.Generic;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerCapture : MonoBehaviour
    {
        private bool _canCapture;
        public PlayerInput playerInput;

        [HideInInspector] public int score;

        [HideInInspector] public List<CapturePoint> inventory;
        [HideInInspector] public CapturePoint currentCapturePoint;

        public PlayerColor color;
        
        [Header("Interface Elements")]
        public TextMeshProUGUI actionBar;
        public TextMeshProUGUI captureHealth;

        // Called every time a player presses the 'Capture' binding.
        public void Capture(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            if (currentCapturePoint == null) return;
            currentCapturePoint.AttemptCapture(this);
        }

        /* When a player enters a 'CapturePoint' on a planet, set it
         to their currently selected CapturePoint in the script so that
         if they press any buttons (above) it will call the function on
         that CapturePoint. */
        private void OnTriggerEnter(Collider other)
        {
            // If the entered trigger is not a capture point, ignore it.
            if (!other.CompareTag("CapturePoint")) return;
            RefreshInterfaceElements();
            
            currentCapturePoint = other.GetComponent<CapturePoint>();
        }

        /* When a player exits a 'CapturePoint' on a
         planet, nullify their current capture point. */
        private void OnTriggerExit(Collider other)
        {
            // If the entered trigger is not a capture point, ignore it.
            if (!other.CompareTag("CapturePoint")) return;
            
            actionBar.gameObject.SetActive(false);
            captureHealth.gameObject.SetActive(false);

            currentCapturePoint = null;
        }

        public void RefreshInterfaceElements()
        {
            if (currentCapturePoint == null)
            {
                captureHealth.gameObject.SetActive(false);
                return;
            }
            
            captureHealth.SetText(currentCapturePoint.health.ToString("N0"));
            captureHealth.gameObject.SetActive(true);
            
            if (currentCapturePoint.currentCaptor == this)
            {
                // If the planet's owner is the current player
                actionBar.gameObject.SetActive(false);
            }
            else if (currentCapturePoint.currentCaptor != null)
            {
                // If the planet's owner is NOT the current player
                // but it HAS one
                actionBar.SetText("Destroy Host Tree [F]");
                actionBar.gameObject.SetActive(true); }
            else
            {
                // If the planet's owner is NOT the current player
                // but it DOES NOT have one
                actionBar.SetText("Plant New Tree [F]");
                actionBar.gameObject.SetActive(true);
            }
        }

        /* Called once every second. Increments the player's
         score by the amount of Capture Points they currently own. */
        public void IncrementScore()
        {
            score += inventory.Count;
            Debug.Log(name + "'s score changed to " + score);
            
            // If the player achieves the maximum score
            if (score >= PlayerManager.Instance.scoreboard.maxScore)
            {
                GameOver.Instance.Trigger(playerInput.playerIndex);
            }
        }
    }
}
