using System.Collections.Generic;
using UnityEngine;

namespace Interfaces
{
    /// <summary>
    /// Creates and manages all PlayerScoreCard elements.
    /// </summary>
    public class ScoreUIManager : MonoBehaviour
    {
        public static ScoreUIManager Instance { get; private set; }

        [Header("UI Elements")]
        // Assigned manually via Unity Editor
        public Transform scoreCardsContainer;

        public PlayerScoreCard scoreCardPrefab;

        private List<PlayerScoreCard> _playerScoreCards = new(); // List to hold the score cards for each player

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject); Not sure we need this 
            }
            else Destroy(gameObject);
        }

        public void CreateAllScoreCards()
        {
            Debug.Log("Killing all score cards...");
            while (scoreCardsContainer.childCount > 0)
            {
                DestroyImmediate(scoreCardsContainer.GetChild(0).gameObject);
            }
            
            Debug.Log("Creating all score cards...");
            foreach (var player in PlayerManager.Instance.scoreboard.players)
            {
                Instance.CreateScoreCard(player.playerInput.playerIndex + 1, 0);
            }
        }
        
        private void CreateScoreCard(int playerID, int initialScore)
        {
            var newScoreCard = Instantiate(scoreCardPrefab, scoreCardsContainer);
            newScoreCard.SetPlayerID(playerID);
            newScoreCard.SetPlayerScore(initialScore);
            _playerScoreCards.Add(newScoreCard);
        }

        // Method to refresh the scores for all players
        public void Refresh()
        {
            // Assuming PlayerManager.Instance.scoreboard.players is a list of player scores
            for (int i = 0; i < PlayerManager.Instance.scoreboard.players.Count; i++)
            {
                _playerScoreCards[i].SetPlayerScore(PlayerManager.Instance.scoreboard.players[i].score);
            }
        }
    }
}