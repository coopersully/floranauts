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
    public List<PlayerController> players;

    [Header("Scoring System")]
    public PlayerScoreManager scoreboard;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void RegisterPlayer(PlayerInput playerInput)
    {
        var playerObject = playerInput.gameObject;
        var controller = playerObject.GetComponent<PlayerController>();
        controller.SetBodyActive(false);

        players.Add(controller);
        scoreboard.RegisterPlayerScoreboard(playerInput);
    }

    public void CarryPlayersToScene(int buildIndex, Action afterwards = null)
    {
        LoadingScreen.Instance.Load(buildIndex, players, afterwards);
    }

    public void SetAllPlayerBodiesActive(bool exists)
    {
        foreach (var player in players) player.SetBodyActive(exists);
    }
}
