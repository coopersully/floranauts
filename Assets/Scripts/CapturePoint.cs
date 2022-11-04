using System;
using Player;
using UnityEngine;

public class CapturePoint : MonoBehaviour
{
    public Planet planet;
    public PlanetType planetType;
    
    public PlayerCapture currentCaptor;
    public GameObject captureIndicator;
    private Material _captureIndicatorMaterial;

    private void Awake()
    {
        _captureIndicatorMaterial = captureIndicator.GetComponent<MeshRenderer>().material;
    }

    public void CaptureAction(PlayerCapture playerCapture)
    {
        if (currentCaptor == playerCapture) return;
        
        if (currentCaptor != null)
        {
            currentCaptor = null;
            planet.Unclaim();
            _captureIndicatorMaterial.color = planet.unclaimedColor;
            return;
        }
        
        planet.Claim(playerCapture);
        _captureIndicatorMaterial.color = playerCapture.playerColor;
        currentCaptor = playerCapture;
    }
    
}
