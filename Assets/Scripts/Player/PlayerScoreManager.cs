using System;
using System.Collections;
using Interfaces;
using UnityEngine;

namespace Player
{
    public class PlayerScoreManager : MonoBehaviour
    {
        [Header("Scoring System")]
        public int maxScore = 100;
        public bool maxScoreAchieved;
        public float incrementTime = 3.0f;
        
        [Header("Players")]
        public PlayerCapture playerOne;
        public PlayerCapture playerTwo;

        [Header("Interfaces")]
        public ScoreManager scoreManager;
        
        public void RestartScoreTicking()
        {
            StopAllCoroutines();
            StartCoroutine(IncrementScores());
        }

        private IEnumerator IncrementScores()
        {
            while (!maxScoreAchieved)
            {
                Debug.Log("Incrementing player scores...");

                // Tell each player to increment their scores
                playerOne.IncrementScore();
                playerTwo.IncrementScore();

                // Update visual UI elements
                scoreManager.Refresh();
                
                // Wait for allotted time before re-looping
                yield return new WaitForSeconds(incrementTime);
            }
        }
    }
}
