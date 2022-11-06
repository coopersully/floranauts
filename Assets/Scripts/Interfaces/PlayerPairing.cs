using System;
using System.Collections;
using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interfaces
{
    public class PlayerPairing : MonoBehaviour
    {
        
        [Header("General & Upper")]
        public GameObject pairingSection;
        public GameObject noPlayersConnected;
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
        
        private void Update()
        {
            RefreshUI();
            
            // Enable the "no players" placeholder text if there are... no players lol
            noPlayersConnected.SetActive(_players < 1);

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
            
            var newPlayer = Instantiate(playerCard, playersReady, false);
            newPlayer.title.SetText("Player " + (playerInput.playerIndex + 1));
            newPlayer.subtitle.SetText(playerInput.currentControlScheme);

            var playerColor = playerInput.playerIndex switch
            {
                0 => Color.yellow,
                1 => Color.green,
                3 => Color.magenta,
                4 => Color.blue,
                _ => throw new ArgumentOutOfRangeException()
            };

            newPlayer.background.color = playerColor;
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
            gameObject.SetActive(false); // Disable this script
        }
    }
}
