using System;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Players")]
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
        switch (playerInput.playerIndex)
        {
            case 0:
                playerOne = playerInput.gameObject;
                break;
            case 1:
                playerTwo = playerInput.gameObject;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
