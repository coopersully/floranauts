using System;
using System.Collections;
using Audio;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interfaces
{
    public class PlayerPairing : MonoBehaviour
    {

        public static string[] Usernames =
        {
            "Alpha",
            "Bravo",
            "Charlie",
            "Delta",
            "Echo",
            "Foxtrot",
            "Golf",
            "Hotel",
            "India",
            "Juliet",
            "Kilo",
            "Lima",
            "Mike",
            "November",
            "Oscar",
            "Papa",
            "Quebec",
            "Romeo",
            "Sierra",
            "Tango",
            "Uniform",
            "Victor",
            "Whiskey",
            "X-ray",
            "Yankee",
            "Zulu"
        };
        
        [Header("General & Upper")]
        public GameObject pairingSection;
        public TextMeshProUGUI numPlayersConnected;
        
        [Header("Scroll View")]
        public PlayerCard playerCard;
        public Transform playersReady;

        [Header("Countdown UI")]
        private bool _countingDown = false;
        public int countdownStart = 3;
        public GameObject countdownOverlay;
        public TextMeshProUGUI countdownTimer;

        private int _maxPlayers;
        private int _players;

        private void OnEnable()
        {
            // Disable HUD
            PauseManager.Instance.SetHUDVisibility(false);
            
            // Restrict from using pause menu controls
            PauseManager.Instance.restrictions.Add(gameObject);
            
            // Allow new players to join
            PlayerInputManager.instance.EnableJoining();
        }

        private void Update()
        {
            RefreshUI();

            if (_players < _maxPlayers)
            {
                // If not all players are connected to the game
                pairingSection.SetActive(true);
                return;
            }

            // If all players have connected to the game
            if (!_countingDown) StartCoroutine(Dismiss());
        }

        public void OnPlayerJoined(PlayerInput playerInput)
        {
            AudioManager.Instance.ui.Select03();
            
            var title = Usernames[playerInput.playerIndex];
            
            var newPlayer = Instantiate(playerCard, playersReady, false);
            newPlayer.index.SetText((playerInput.playerIndex + 1).ToString());
            newPlayer.title.SetText(title);
            newPlayer.subtitle.SetText("USING " + playerInput.currentControlScheme);

            playerInput.gameObject.name = title;
            
            // Re-color player card
            var primary = PlayerColor.GetPrimary(playerInput.playerIndex);
            var secondary = PlayerColor.GetSecondary(playerInput.playerIndex);

            newPlayer.background.color = primary;
            foreach (var icon in newPlayer.icons)
            {
                icon.color = secondary;
            }
            //newPlayer.title.color = secondary;
            newPlayer.subtitle.color = secondary;
            
            PlayerManager.Instance.AddPlayer(playerInput);
        }

        private void RefreshUI()
        {
            _maxPlayers = PlayerInputManager.instance.maxPlayerCount;
            _players = PlayerInputManager.instance.playerCount;
            numPlayersConnected.SetText(_players + "/" + _maxPlayers + " players connected");
        }

        private IEnumerator Dismiss()
        {
            // Enable countdown overlay
            _countingDown = true;
            countdownOverlay.SetActive(true);
            
            // Restrict new players from joining
            PlayerInputManager.instance.DisableJoining();

            // Count down each second and set the timer's text
            for (int i = countdownStart; i > 0; i--)
            {
                AudioManager.Instance.ui.Click03();
                countdownTimer.SetText(i.ToString("N0"));
                yield return new WaitForSeconds(1.0f);
            }
            
            // Disable everything including this component
            _countingDown = false;
            pairingSection.SetActive(false);
            
            // Begin counting scores
            PlayerManager.Instance.scoreboard.RestartScoreTicking();
            
            // Enable HUD
            PauseManager.Instance.SetHUDVisibility(true);
            
            // Remove restriction from using pause menu
            PauseManager.Instance.restrictions.Remove(gameObject);

            gameObject.SetActive(false); // Disable this script
        }
    }
}
