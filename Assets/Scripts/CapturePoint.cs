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
        
        /* If there is already an owner of the current CapturePoint,
         when a player executes the 'Capture Action,' un-claim it
         first instead of instantly claiming it for them. */
        if (currentCaptor != null)
        {
            // Change planet color to unclaimed
            planet.Unclaim();
            _captureIndicatorMaterial.color = planet.unclaimedColor;
            
            // Add point to player's inventory
            currentCaptor.inventory.Remove(this);
            
            // Change the current captor for this CapturePoint
            currentCaptor = null;
            return;
        }
        
        // Change planet color to player's color
        planet.Claim(playerCapture);
        _captureIndicatorMaterial.color = playerCapture.primaryColor;
        
        // Add point to player's inventory
        playerCapture.inventory.Add(this);
        
        // Change the current captor for this CapturePoint
        currentCaptor = playerCapture;
    }
    
}
