using Player;
using TMPro;
using UnityEngine;

namespace Interfaces
{
    public class ScoreManager : MonoBehaviour
    {
        [Header("-Players-")]
        public PlayerCapture bluePlayer;
        public PlayerCapture redPlayer;
    
        [Header("-Score UI-")]
        public TextMeshProUGUI bluePlayerScoreUI;
        public TextMeshProUGUI redPlayerScoreUI;

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
            redPlayerScoreUI.SetText("Score: " + redPlayer.score.ToString("N0"));
        }
    }
}