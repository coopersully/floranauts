using Player;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CapturePoint : MonoBehaviour
{
    public Planet planet;

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
    
    [Header("Capture Properties")]
    [HideInInspector] public PlayerCapture currentCaptor;
    public int health;
    public int maxHealth = 10;
    
    private void Awake()
    {
        // Initialize planet health
        health = maxHealth;
        
        // Assign 'Material' components for the tree
        _moundMaterial = mound.material;
        _trunkMaterial = trunk.material;
        _fruitMaterial = fruit.material;
        _leavesMaterial = leaves.material;
    }

    public void AttemptCapture(PlayerCapture playerCapture)
    {
        if (currentCaptor == playerCapture) return;

        // Decrement the health of the planet
        health -= 1;
        
        // If the health has reached zero, capture the planet.
        if (health <= 0)
        {
            Capture(playerCapture);
            health = maxHealth;
        }
        
        // Set the player's UI element to display current health
        PlayerManager.Instance.scoreboard.playerOne.RefreshInterfaceElements();
        PlayerManager.Instance.scoreboard.playerTwo.RefreshInterfaceElements();
    }

    private void Capture(PlayerCapture playerCapture)
    {
        /* If there is already an owner of the current CapturePoint,
         un-claim it first instead of instantly claiming it for them. */
        if (currentCaptor != null) RemoveCaptor();
        else AddCaptor(playerCapture);
    }

    private void RemoveCaptor()
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
        PlayerManager.Instance.scoreboard.playerOne.RefreshInterfaceElements();
        PlayerManager.Instance.scoreboard.playerTwo.RefreshInterfaceElements();
    }

    
    private void AddCaptor(PlayerCapture playerCapture)
    {
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
        PlayerManager.Instance.scoreboard.playerOne.RefreshInterfaceElements();
        PlayerManager.Instance.scoreboard.playerTwo.RefreshInterfaceElements();
    }

    private void SetTreeVisibility(bool isVisible)
    {
        claimParticles.Play();
        
        trunk.gameObject.SetActive(isVisible);
        fruit.gameObject.SetActive(isVisible);
        leaves.gameObject.SetActive(isVisible);
    }
}
