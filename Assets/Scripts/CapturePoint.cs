using System;
using Player;
using UnityEngine;

public class CapturePoint : MonoBehaviour
{
    public Planet planet;
    public PlayerCapture currentCaptor;

    public bool ghost;
    
    [Header("Tree Elements")]
    public MeshRenderer mound;
    public MeshRenderer trunk;
    public MeshRenderer fruit;
    public MeshRenderer leaves;
    
    private Material _moundMaterial;
    private Material _trunkMaterial;
    private Material _fruitMaterial;
    private Material _leavesMaterial;
    
    private void Awake()
    {
        _moundMaterial = mound.material;
        _trunkMaterial = trunk.material;
        _fruitMaterial = fruit.material;
        _leavesMaterial = leaves.material;
    }

    private void Update()
    {
        if (ghost) Ghost();
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
            _fruitMaterial.color = planet.unclaimedColor;
            _leavesMaterial.color = planet.unclaimedColor;

            // Add point to player's inventory
            currentCaptor.inventory.Remove(this);
            
            // Change the current captor for this CapturePoint
            currentCaptor = null;
            return;
        }
        
        // Change planet color to player's color
        planet.Claim(playerCapture);
        _fruitMaterial.color = playerCapture.color.primary;
        _leavesMaterial.color = playerCapture.color.secondary;

        // Add point to player's inventory
        playerCapture.inventory.Add(this);
        
        // Change the current captor for this CapturePoint
        currentCaptor = playerCapture;
    }

    private void Ghost()
    {
        trunk.gameObject.SetActive(false);
        fruit.gameObject.SetActive(false);
        leaves.gameObject.SetActive(false);
    }
    
}
