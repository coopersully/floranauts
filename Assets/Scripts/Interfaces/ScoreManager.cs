using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Interfaces
{
    public class ScoreManager : MonoBehaviour
    {
        [Header("-Players-")]
        public PlayerCapture bluePlayer;
        public PlayerCapture redPlayer;
    
        [Header("-Score UI-")]
        public TextMeshProUGUI bluePlayerScoreUI;
        public Slider bluePlayerScoreBar;
        public TextMeshProUGUI redPlayerScoreUI;
        public Slider redPlayerScoreBar;

        public void RefreshPlayers()
        {
            // Initialize players
            var allPlayers = FindObjectsOfType<PlayerCapture>();
            foreach (var player in allPlayers)
            {
                switch (player.playerInput.playerIndex)
                {
                    case 0: // First player
                        bluePlayer = player;
                        break;
                    case 1: // Second player
                        redPlayer = player;
                        break;
                }
            }
        }

        public void Update()
        {
            bluePlayerScoreUI.SetText("Score: " + bluePlayer.score.ToString("N0"));
            bluePlayerScoreBar.value = bluePlayer.score;
            redPlayerScoreUI.SetText("Score: " + redPlayer.score.ToString("N0"));
            redPlayerScoreBar.value = redPlayer.score;
        }
    }
}