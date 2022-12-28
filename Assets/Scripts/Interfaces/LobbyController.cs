using System.Collections;
using System.Collections.Generic;
using Audio;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

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

        public List<PlayerCard> cards = new ();
        
        [Header("General & Upper")]
        public TextMeshProUGUI captainName;
        public TextMeshProUGUI numOccupants;
        
        [FormerlySerializedAs("playerCard")] [Header("Scroll View")]
        public PlayerCard playerCardPrefab;
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
        
        private PlayerCard CreatePlayerCard(PlayerInput playerInput)
        {
            var uid = playerInput.user.id;
            var index = playerInput.playerIndex;
            var indexDeco = (playerInput.playerIndex + 1).ToString();
            
            var playerCard = Instantiate(playerCardPrefab, content, false);
            
            // Update player's card information
            var alias = Usernames[index];
            playerCard.uid = uid;
            playerCard.index.SetText(indexDeco);
            playerCard.title.SetText(alias);
            playerCard.subtitle.SetText("USING " + playerInput.currentControlScheme);
            
            // Update player's card colors
            var colorPrimary = PlayerColor.GetPrimary(index);
            var colorSecondary = PlayerColor.GetSecondary(index);
            
            playerCard.background.color = colorPrimary;
            foreach (var icon in playerCard.icons) icon.color = colorSecondary;
            
            Debug.Log("Card for player (UID " + uid + ") was created.");
            
            cards.Add(playerCard);
            return playerCard;
        }

        private void RemovePlayerCard(PlayerInput playerInput)
        {
            var uid = playerInput.user.id;
            var playerCard = cards.Find(card => card.uid == uid);
            
            Destroy(playerCard.gameObject);
            cards.Remove(playerCard);
            
            Debug.Log("Card for player (UID " + uid + ") was removed.");
        }

        public void OnPlayerJoined(PlayerInput playerInput)
        {
            var uid = playerInput.user.id;
            var playerIndex = playerInput.playerIndex;
            var playerIndexDeco = (playerIndex + 1).ToString();

            Debug.Log("Player (UID " + uid + ") joined the lobby.");

            var playerCard = CreatePlayerCard(playerInput);

            // Change player's GameObject name
            playerInput.gameObject.name = "Player " + playerIndexDeco;

            // Register player in game session
            PlayerManager.Instance.RegisterPlayer(playerInput);
            
            // Play "join" audio cue & update interface elements
            AudioManager.Instance.ui.Select03();
            RefreshUI();
            
            // If the maximum amount of players is met, begin the countdown
            if (_players == _maxPlayers) StartCountdown();
        }

        public void OnPlayerLeave(PlayerInput playerInput)
        {
            RemovePlayerCard(playerInput);
            RefreshUI();
        }

        private void RefreshUI()
        {
            _maxPlayers = PlayerInputManager.instance.maxPlayerCount;
            _players = PlayerInputManager.instance.playerCount;

            var captain = PlayerManager.Instance.players[0];
            if (captain != null)
            {
                captainName.SetText(captain.name);
            }
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
            PlayerManager.Instance.CarryPlayersToScene(2, CheaplyEnableAllPlayers);
        }

        private static void CheaplyEnableAllPlayers()
        {
            PlayerManager.Instance.SetAllPlayerBodiesActive(true);
        }

        public void LeaveLobby()
        {
            // Unlock cursors
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            // // Destroy other objects
            // var playerManager = FindObjectOfType<PlayerManager>().gameObject;
            // var currentScene = SceneManager.GetActiveScene();
            // SceneManager.MoveGameObjectToScene(playerManager, currentScene);
            // PlayerInputManager.instance.
            
            // Load main menu scene
            LoadingScreen.Instance.Load(0);
        }
    }
}
