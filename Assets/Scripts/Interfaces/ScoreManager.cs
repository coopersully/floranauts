using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Interfaces
{
    public class ScoreManager : MonoBehaviour
    {
        [Header("UI Elements")]
        public TextMeshProUGUI playerOneText;
        public Slider playerOneProgress;
        public TextMeshProUGUI playerTwoText;
        public Slider playerTwoProgress;
        
        public void Refresh()
        {
            playerOneText.SetText(PlayerManager.Instance.scoreboard.playerOne.score.ToString("N0"));
            playerOneProgress.value = PlayerManager.Instance.scoreboard.playerOne.score;
            
            playerTwoText.SetText(PlayerManager.Instance.scoreboard.playerTwo.score.ToString("N0"));
            playerTwoProgress.value = PlayerManager.Instance.scoreboard.playerTwo.score;
        }
    }
}