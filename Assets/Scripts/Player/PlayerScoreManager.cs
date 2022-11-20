using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerScoreManager : MonoBehaviour
    {
        [Header("Scoring System")]
        public int maxScore = 100;
        public bool maxScoreAchieved;
        
        [Header("Players")]
        public PlayerCapture playerOne;
        public PlayerCapture playerTwo;
        
        private void Awake()
        {
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
