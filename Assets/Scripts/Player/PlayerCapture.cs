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

        public TextMeshProUGUI blueCaptureTextPrompt;
        public TextMeshProUGUI redCaptureTextPrompt;
        

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

            //Shows capture planet text prompt
            if (CaptureTextPrompt.promptName== "BlueCaptureTextPrompt")
            {
                blueCaptureTextPrompt.gameObject.SetActive(true);
            }
            else
            {
                redCaptureTextPrompt.gameObject.SetActive(true);
            }
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
        public void IncrementScore()
        {
            score += inventory.Count;
            
            // If the player achieves the maximum score
            if (score >= PlayerManager.Instance.scoreboard.maxScore)
            {
                GameOver.Instance.Trigger(playerInput.playerIndex);
            }
        }
    }
}
