using System.Collections.Generic;
using Audio;
using Player;
using UnityEngine;

namespace Planets
{
    [RequireComponent(typeof(Collider))]
    public class CapturePoint : MonoBehaviour
    {
        public Planet planet;
        public PlanetType item = PlanetType.None;

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
        [HideInInspector] public PlayerCapture owner;
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
            
            // Activate ghostly tree
            SetTreeColor(null);
        }

        public void AttemptCapture(PlayerCapture playerCapture)
        {
            if (owner == playerCapture) return;

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
            if (owner != null) RemoveCaptor();
            else AddCaptor(playerCapture);
        }

        private void RemoveCaptor()
        {
            AudioManager.Instance.fx.UnCapturePlanet();
            
            // Change planet color to unclaimed
            planet.Unclaim();
            SetTreeColor(null);

            // Add point to player's inventory
            owner.inventory.Remove(this);
            
            // Remove item from previous owner
            owner.playerItems.RemoveItem(item);
            
            // Set the tree's visibility
            SetTreeVisibility(false);
            
            // Change the current captor for this CapturePoint
            owner = null;
            
            // Refresh everyone's action bars
            PlayerManager.Instance.scoreboard.playerOne.RefreshInterfaceElements();
            PlayerManager.Instance.scoreboard.playerTwo.RefreshInterfaceElements();
        }

    
        private void AddCaptor(PlayerCapture playerCapture)
        {
            AudioManager.Instance.fx.CapturePlanet();
            
            // Change planet color to player's color
            planet.Claim(playerCapture);
            SetTreeColor(playerCapture);

            // Add point to player's inventory
            playerCapture.inventory.Add(this);
            
            // Add item to new owner
            playerCapture.playerItems.AddItem(item);
        
            // Set the tree's visibility
            SetTreeVisibility(true);
        
            // Change the current captor for this CapturePoint
            owner = playerCapture;
        
            // Refresh everyone's action bars
            PlayerManager.Instance.scoreboard.playerOne.RefreshInterfaceElements();
            PlayerManager.Instance.scoreboard.playerTwo.RefreshInterfaceElements();
        }

        private void SetTreeVisibility(bool isVisible)
        {
            claimParticles.Play();
            fruit.gameObject.SetActive(isVisible);
        }

        private void SetTreeColor(PlayerCapture playerCapture)
        {
            if (playerCapture != null)
            {
                fruit.material.color = playerCapture.color.primary;
                leaves.material.color = playerCapture.color.secondary;
                trunk.material.color = new Color32(99, 69, 58, 255);
            }
            else
            {
                leaves.material.color = new Color32(255, 255, 255, 10);
                trunk.material.color = new Color32(255, 255, 255, 10);
            }
        }
    }
}
