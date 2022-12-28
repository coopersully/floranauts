using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Interfaces
{
    public class GameOver : MonoBehaviour
    {
        public static GameOver Instance;

        public GameObject playerOneWin;
        public GameObject playerTwoWin;

        public Slider playerOneScore;
        public Slider playerTwoScore;

        public Animator animator;
        private static readonly int End = Animator.StringToHash("end");

        public TextMeshProUGUI countdownText;
        public int secondsBeforeReturn;
        private int _countdown;

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        public void Trigger(int winnerIndex)
        {
            // Alert ScoreManager singleton of winner
            PlayerManager.Instance.scoreboard.maxScoreAchieved = true;
            
            // Fix pause menu
            PauseManager.Instance.Resume();
            PauseManager.Instance.SetHUDVisibility(false);
            PauseManager.Instance.restrictions.Add(gameObject);

            // Set sliders to their scores
            // TODO: Fix for multiplayer
            //playerOneScore.SetValueWithoutNotify( (float) PlayerManager.Instance.scoreboard.playerOne.score );
            //playerTwoScore.SetValueWithoutNotify( (float) PlayerManager.Instance.scoreboard.playerTwo.score );
            
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
            
            // // Unlock cursors
            // Cursor.visible = true;
            // Cursor.lockState = CursorLockMode.None;
            
            // Start countdown
            StartCoroutine(CountdownToMainMenu());
        }
        
        public void MainMenu()
        {
            // Unlock cursors
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            // Load main menu scene
            LoadingScreen.Instance.Load(0);
        }
        
        private IEnumerator CountdownToMainMenu()
        {
            yield return new WaitForSeconds(2.0f);
            countdownText.gameObject.SetActive(true);
            
            _countdown = secondsBeforeReturn;
            while (_countdown > 0)
            {
                countdownText.SetText("Returning to Main Menu in " + _countdown + " seconds...");
                _countdown--;
                yield return new WaitForSeconds(1.0f);
            }
            MainMenu();
        }
        
    }
}
