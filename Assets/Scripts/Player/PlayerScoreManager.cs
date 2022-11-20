using System;
using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerScoreManager : MonoBehaviour
    {
        public static PlayerScoreManager Instance;
        
        public int maxScore = 100;
        public bool maxScoreAchieved;
        
        [Header("Players")]
        public PlayerCapture playerOne;
        public PlayerCapture playerTwo;
        
        private void Awake()
        {
            if (Instance == null) Instance = this;
            
            playerOne = PlayerManager.Instance.playerOne.GetComponent<PlayerCapture>();
            playerTwo = PlayerManager.Instance.playerTwo.GetComponent<PlayerCapture>();
            StartCoroutine(IncrementScores());
        }

        private IEnumerator IncrementScores()
        {
            while (!maxScoreAchieved)
            {
                playerOne.IncrementScore();
                playerTwo.IncrementScore();
                yield return new WaitForSeconds(3.0f);
            }
        }
    }
}
