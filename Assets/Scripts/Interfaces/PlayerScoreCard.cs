using TMPro;
using UnityEngine;

namespace Interfaces
{
    /// <summary>
    /// A miniature script to manage the UI elements within each player's
    /// designated 'score card' in game.
    /// </summary>
    public class PlayerScoreCard : MonoBehaviour
    {
        // Assigned manually via Unity Editor
        [SerializeField] private TextMeshProUGUI playerID;
        [SerializeField] private TextMeshProUGUI playerScore;

        public void SetPlayerID(int newPlayerID)
        {
            playerID.SetText("Player" + newPlayerID + " - ");
        }
        
        public void SetPlayerScore(int newPlayerScore)
        {
            playerScore.SetText(newPlayerScore.ToString("N0"));
        }
        
        public void SetPlayerScore(double newPlayerScore)
        {
            playerScore.SetText(newPlayerScore.ToString("N0"));
        }
    }
}