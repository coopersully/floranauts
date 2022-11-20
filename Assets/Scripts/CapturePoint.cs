using Player;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CapturePoint : MonoBehaviour
{
    public Planet planet;
    [HideInInspector] public PlayerCapture currentCaptor;
    
    [Header("Tree Elements")]
    public MeshRenderer mound;
    public MeshRenderer trunk;
    public MeshRenderer fruit;
    public MeshRenderer leaves;
    public ParticleSystem claimParticles;
    
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
            
            // Set the tree's visibility
            SetTreeVisibility(false);
            
            // Change the current captor for this CapturePoint
            currentCaptor = null;
            
            // Refresh everyone's action bars
            PlayerManager.Instance.scoreboard.playerOne.RefreshActionBar();
            PlayerManager.Instance.scoreboard.playerTwo.RefreshActionBar();
            return;
        }
        
        // Change planet color to player's color
        planet.Claim(playerCapture);
        _fruitMaterial.color = playerCapture.color.primary;
        _leavesMaterial.color = playerCapture.color.secondary;

        // Add point to player's inventory
        playerCapture.inventory.Add(this);
        
        // Set the tree's visibility
        SetTreeVisibility(true);
        
        // Change the current captor for this CapturePoint
        currentCaptor = playerCapture;
        
        // Refresh everyone's action bars
        PlayerManager.Instance.scoreboard.playerOne.RefreshActionBar();
        PlayerManager.Instance.scoreboard.playerTwo.RefreshActionBar();
    }

    private void SetTreeVisibility(bool isVisible)
    {
        claimParticles.Play();
        
        trunk.gameObject.SetActive(isVisible);
        fruit.gameObject.SetActive(isVisible);
        leaves.gameObject.SetActive(isVisible);
    }
}
