using System;
using System.Collections.Generic;
using Interfaces;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Players")]
    public List<GameObject> players;
    public GameObject playerOne;
    public GameObject playerTwo;

    [Header("Scoring System")]
    public PlayerScoreManager scoreboard;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void AddPlayer(PlayerInput playerInput)
    {
        var playerObject = playerInput.gameObject;

        players.Add(playerObject);
        switch (playerInput.playerIndex)
        {
            case 0:
                playerOne = playerObject;
                scoreboard.playerOne = playerOne.GetComponent<PlayerCapture>();
                break;
            case 1:
                playerTwo = playerObject;
                scoreboard.playerTwo = playerTwo.GetComponent<PlayerCapture>();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void CarryPlayersToScene(int buildIndex)
    {
        LoadingScreen.Instance.Load(buildIndex, players);
    }
}
