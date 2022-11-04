using System.Collections;
using TMPro;
using UnityEngine;

/*Written by Thomas Walker
 11/1/2022
 
 Things to Work On
 - Really hard to trigger the planet capture in OnTriggerEnter
 - Error trying to access the text in the Score UI
 
 - Does there need to be a copy of everything for each player? How are we doing that?
 - What happens when you win?
 */

namespace Player
{
    public class PlayerScoreManager : MonoBehaviour
    {
        [Header("-Planets Captured by Players-")]
        public static int BluePlayerPlanetsCount;
        public static int RedPlayerPlanetsCount;

        // Player scores
        private int _bluePlayerScore;
        private int _redPlayerScore;

        [Header("-Score UI-")]
        public TextMeshProUGUI bluePlayerScoreUI;
        public TextMeshProUGUI redPlayerScoreUI;

        private void Awake()
        {
            _bluePlayerScore = 0;
            _redPlayerScore = 0;
            BluePlayerPlanetsCount = 0;
            RedPlayerPlanetsCount = 0;
        
            // Sets score UI and begins tracking score
            StartCoroutine(IterateScore());
            UpdateScoreUI();
        }

        private void Update()
        {
        
            UpdateScoreUI();
        
            // Keeps the blue player's score from going below zero and activates a win if they reach 100 points
            if (_bluePlayerScore < 0)
            {
                _bluePlayerScore = 0;
            } else if (_bluePlayerScore >= 100)
            {
                BlueWin();
            }
        
            // Keeps the red player's score from going below zero and activates a win if they reach 100 points
            if (_redPlayerScore < 0)
            {
                _redPlayerScore = 0;
            } else if (_redPlayerScore >= 100)
            {
                RedWin();
            }
        }

        // Updates the score UI
        private void UpdateScoreUI()
        {
            bluePlayerScoreUI.SetText("Score: " + BluePlayerPlanetsCount);
            redPlayerScoreUI.SetText("Score: " + RedPlayerPlanetsCount);
        }
    
        // Adds total planets captured to each players' score
        private IEnumerator IterateScore()
        {
            while (true)
            {
                _bluePlayerScore += BluePlayerPlanetsCount;
                _redPlayerScore += RedPlayerPlanetsCount;
                yield return new WaitForSeconds(1.0f);
            }
        }

        private void BlueWin()
        {
            Debug.Log("Blue Player wins");
            StopCoroutine(IterateScore());
        }
    
        private void RedWin()
        {
            Debug.Log("Red Player wins");
            StopCoroutine(IterateScore());
        }
    }
}
