using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>
    ///
    /// PersistentObject, 1 of 1
    /// 
    /// Manages the scores for players within the game. It handles the registration of player scores, incrementing
    /// scores over time, and updating the associated UI elements. It also controls the score ticking process, allowing
    /// for the scores to be incremented at regular intervals, and provides functionality to restart the score ticking
    /// process.
    /// </summary>
    public class PlayerScoreManager : MonoBehaviour
    {
        [Header("Scoring System")] public int maxScore = 100;
        public bool maxScoreAchieved;
        public float incrementTime = 3.0f;
        [SerializeField] private ScoreUIManager scoreUIManager;

        [Header("Players")] public List<PlayerCapture> players;

        public void RestartScoreTicking()
        {
            StopAllCoroutines();
            
            scoreUIManager = GameObject.FindObjectOfType<ScoreUIManager>();
            scoreUIManager.CreateAllScoreCards();

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
            while (true)
            {
                // Tell each player to increment their scores
                foreach (var player in players) player.IncrementScore();

                // Update visual UI elements
                ScoreUIManager.Instance.Refresh();
                
                // Wait for allotted time before re-looping
                yield return new WaitForSeconds(incrementTime);
            }
        }
    }
}