using UnityEngine;

namespace Interfaces
{
    public class GameOver : MonoBehaviour
    {
        public static GameOver Instance;

        public GameObject playerOneWin;
        public GameObject playerTwoWin;

        public Animator animator;
        private static readonly int End = Animator.StringToHash("end");

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        public void Trigger(int winnerIndex)
        {
            // Alert ScoreManager singleton of winner
            ScoreManager.Instance.hasWon = true;
            
            // Fix pause menu
            PauseManager.Instance.SetHUDVisibility(false);
            PauseManager.Instance.restrictions.Add(gameObject);
            
            // Unlock cursors
            Cursor.lockState = CursorLockMode.None;
            
            // Determine winner banner reveal
            switch (winnerIndex)
            {
                case 0: // First player
                    playerOneWin.SetActive(true);
                    playerTwoWin.SetActive(false);
                    break;
                case 1: // Second player
                    playerOneWin.SetActive(false);
                    playerTwoWin.SetActive(true);
                    break;
            }
            
            // Open screen w/ animator
            animator.SetTrigger(End);
        }
    }
}
