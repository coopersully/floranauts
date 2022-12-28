using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerScoreManager : MonoBehaviour
    {
        [Header("Scoring System")]
        public int maxScore = 100;
        public bool maxScoreAchieved;
        public float incrementTime = 3.0f;

        [Header("Players")]
        public List<PlayerCapture> players;

        [Header("Interfaces")]
        public ScoreManager scoreManager;
        
        public void RestartScoreTicking()
        {
            StopAllCoroutines();
            StartCoroutine(IncrementScores());
        }

        public void RegisterPlayerScoreboard(PlayerInput playerInput)
        {
            var playerCapture = playerInput.gameObject.GetComponentInChildren<PlayerCapture>(true);
            if (playerCapture == null)
            {
                Debug.Log(playerInput.gameObject.name + " did not have a PlayerCapture component.");
                return;
            }
            players.Add(playerCapture);
        }

        private IEnumerator IncrementScores()
        {
            while (!maxScoreAchieved)
            {
                Debug.Log("Incrementing player scores...");

                // Tell each player to increment their scores
                foreach (var player in players) player.IncrementScore();

                // Update visual UI elements
                Debug.Log("ScoreManager not refreshed; check TO-DO");
                //scoreManager.Refresh();
                
                // Wait for allotted time before re-looping
                yield return new WaitForSeconds(incrementTime);
            }
        }
    }
}
