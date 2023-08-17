using System;
using System.Collections.Generic;
using Interfaces;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///
/// PersistentObject, 1 of 1
/// 
/// Responsible for managing players within the game, including registration, camera layer assignment, scoring system
/// integration, and transitioning players between scenes. It acts as a central hub for player-related operations,
/// ensuring that player data and states are handled consistently throughout the game.
/// </summary>
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Players")] public List<PlayerController> players;

    [Header("Scoring System")] public PlayerScoreManager scoreboard;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void RegisterPlayer(PlayerInput playerInput)
    {
        var playerObject = playerInput.gameObject;
        var controller = playerObject.GetComponent<PlayerController>();
        controller.SetBodyActive(false);

        var player01 = LayerMask.NameToLayer("PlayerCam1");
        var player02 = LayerMask.NameToLayer("PlayerCam2");
        var player03 = LayerMask.NameToLayer("PlayerCam3");
        var player04 = LayerMask.NameToLayer("PlayerCam4");

        // Assign camera layer-masks
        switch (playerInput.playerIndex)
        {
            case 0: // Player 1
                // Assign layer mask
                playerInput.camera.gameObject.layer = player01;

                // Remove opposing players from culling mask
                RemoveLayerFromCullingMask(playerInput.camera, player02);
                RemoveLayerFromCullingMask(playerInput.camera, player03);
                RemoveLayerFromCullingMask(playerInput.camera, player04);
                break;
            case 1: // Player 2
                // Assign layer mask
                playerInput.camera.gameObject.layer = player02;

                // Remove opposing players from culling mask
                RemoveLayerFromCullingMask(playerInput.camera, player01);
                RemoveLayerFromCullingMask(playerInput.camera, player03);
                RemoveLayerFromCullingMask(playerInput.camera, player04);
                break;
            case 2: // Player 3
                // Assign layer mask
                playerInput.camera.gameObject.layer = player03;

                // Remove opposing players from culling mask
                RemoveLayerFromCullingMask(playerInput.camera, player01);
                RemoveLayerFromCullingMask(playerInput.camera, player02);
                RemoveLayerFromCullingMask(playerInput.camera, player04);
                break;
            case 3: // Player 4
                // Assign layer mask
                playerInput.camera.gameObject.layer = player04;

                // Remove opposing players from culling mask
                RemoveLayerFromCullingMask(playerInput.camera, player01);
                RemoveLayerFromCullingMask(playerInput.camera, player02);
                RemoveLayerFromCullingMask(playerInput.camera, player03);
                break;
        }

        players.Add(controller);
        scoreboard.RegisterPlayerScoreboard(playerInput);
    }

    private static void RemoveLayerFromCullingMask(Camera camera, int layer)
    {
        Debug.Log("Removing layer " + layer + " from " + camera.name + "'s culling mask.");
        camera.cullingMask &= ~(1 << layer);
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