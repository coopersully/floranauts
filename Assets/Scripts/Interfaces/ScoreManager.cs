using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Interfaces
{
    public class ScoreManager : MonoBehaviour
    {

        public static ScoreManager Instance;

        [HideInInspector] public bool hasWon;

        [HideInInspector] public PlayerCapture playerOne;
        [HideInInspector] public PlayerCapture playerTwo;
        
        [Header("-Score UI-")]
        [FormerlySerializedAs("bluePlayerScoreUI")] public TextMeshProUGUI playerOneText;
        [FormerlySerializedAs("bluePlayerScoreBar")] public Slider playerOneProgress;
        [FormerlySerializedAs("redPlayerScoreUI")] public TextMeshProUGUI playerTwoText;
        [FormerlySerializedAs("redPlayerScoreBar")] public Slider playerTwoProgress;

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        public void RefreshPlayers()
        {
            // Initialize players
            var allPlayers = FindObjectsOfType<PlayerCapture>();
            foreach (var player in allPlayers)
            {
                switch (player.playerInput.playerIndex)
                {
                    case 0: // First player
                        playerOne = player;
                        break;
                    case 1: // Second player
                        playerTwo = player;
                        break;
                }
            }
        }

        public void Update()
        {
            playerOneText.SetText("Score: " + playerOne.score.ToString("N0"));
            playerOneProgress.value = playerOne.score;
            
            playerTwoText.SetText("Score: " + playerTwo.score.ToString("N0"));
            playerTwoProgress.value = playerTwo.score;
        }
    }
}