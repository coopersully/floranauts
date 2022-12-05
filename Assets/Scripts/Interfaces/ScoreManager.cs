using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Interfaces
{
    public class ScoreManager : MonoBehaviour
    {
        [Header("UI Elements")]
        public Slider playerOneProgress;
        public Slider playerTwoProgress;
        
        public void Refresh()
        {
            playerOneProgress.value = (float) PlayerManager.Instance.scoreboard.playerOne.score;
            playerTwoProgress.value = (float) PlayerManager.Instance.scoreboard.playerTwo.score;
        }
    }
}