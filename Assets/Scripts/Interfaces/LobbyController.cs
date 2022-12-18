using System.Collections;
using Audio;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interfaces
{
    public class LobbyController : MonoBehaviour
    {
        
        public static readonly string[] Usernames = {
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
        public TextMeshProUGUI captainName;
        public TextMeshProUGUI numOccupants;
        
        [Header("Scroll View")]
        public PlayerCard playerCard;
        public Transform content;

        [Header("Countdown UI")]
        private bool _isCountingDown;
        public int countdownStart = 3;
        public GameObject countdownOverlay;
        public TextMeshProUGUI countdownTimer;

        private int _maxPlayers;
        private int _players;

        private void Awake()
        {
            PlayerInputManager.instance.EnableJoining();
        }

        public void OnPlayerJoined(PlayerInput playerInput)
        {
            var playerIndex = playerInput.playerIndex;
            var playerIndexDeco = (playerIndex + 1).ToString();
            Debug.Log("Player joined the lobby.");

            // Summon player's card
            var card = Instantiate(playerCard, content, false);
            
            // Update player's card information
            var alias = Usernames[playerIndex];
            card.index.SetText(playerIndexDeco);
            card.title.SetText(alias);
            card.subtitle.SetText("USING " + playerInput.currentControlScheme);
            
            // Update player's card colors
            var colorPrimary = PlayerColor.GetPrimary(playerIndex);
            var colorSecondary = PlayerColor.GetSecondary(playerIndex);
            
            card.background.color = colorPrimary;
            foreach (var icon in card.icons) icon.color = colorSecondary;
            card.subtitle.color = colorSecondary;
            
            // Change player's GameObject name
            playerInput.gameObject.name = "P" + playerIndexDeco + " (" + alias + ")";

            // Register player in game session
            PlayerManager.Instance.AddPlayer(playerInput);
            
            // Play "join" audio cue & update interface elements
            AudioManager.Instance.ui.Select03();
            RefreshUI();
            
            // If the maximum amount of players is met, begin the countdown
            if (_players == _maxPlayers) StartCountdown();
        }

        private void RefreshUI()
        {
            _maxPlayers = PlayerInputManager.instance.maxPlayerCount;
            _players = PlayerInputManager.instance.playerCount;
            
            captainName.SetText(PlayerManager.Instance.playerOne.name);
            numOccupants.SetText(_players + "/" + _maxPlayers);
        }

        public void StartCountdown()
        {
            if (_isCountingDown)
            {
                Debug.Log("Captain failed to start game- already starting!");
                AudioManager.Instance.ui.Click01();
                return;
            }

            if (_players < 2)
            {
                Debug.Log("Captain failed to start game- not enough players!");
                AudioManager.Instance.ui.Click01();
                return;
            }
            
            StartCoroutine(CountdownUntilGameStart());
        }

        private IEnumerator CountdownUntilGameStart()
        {
            // Enable countdown overlay
            _isCountingDown = true;
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
            _isCountingDown = false;

            // Begin counting scores
            PlayerManager.Instance.scoreboard.RestartScoreTicking();
            
            // Switch to game scene
            PlayerManager.Instance.CarryPlayersToScene(2);
        }
    }
}
