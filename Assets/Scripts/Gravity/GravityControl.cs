using Player;
using UnityEngine;

namespace Gravity
{
    // This Script goes on the player and tells the planet what to attract.
    public class GravityControl : MonoBehaviour
    {
        private GravityAttractor _planet;
        private PlayerMovement _playerMovement;
        private Rigidbody _rigidbody;

        private bool _shouldRotate = true;
        
        private void Awake()
        {
            _planet = GameObject.FindGameObjectWithTag("Planet1").GetComponent<GravityAttractor>();
            _playerMovement = GetComponent<PlayerMovement>();
            _rigidbody = GetComponent<Rigidbody>();

            // Disable rigidbody built in gravity and freezes rotations so planet can add that itself
            _rigidbody.useGravity = false;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
        

        private void FixedUpdate()
        {
            // Allow this body to be influenced by planet's gravity
            _planet.Attract(_rigidbody);
            if (_shouldRotate) _planet.Rotate(_rigidbody);
        }
        private void OnTriggerEnter(Collider other)
        {
            /* While player is in the air, if hits trigger
             for other planet, gravity switches and player
             slowly rotates. */
            if (!_playerMovement.isGrounded && other.CompareTag("Gravity"))
            {
                _planet = other.GetComponentInParent<GravityAttractor>();
                _planet.rotationSpeed = 1;
                _shouldRotate = true;

            }
            
            /* When Player hits trigger closer to the planet,
             the rotation speed increases, snapping him upright
             in relation to planet. */
            if (other.CompareTag("InnerGravity"))
            {
                _shouldRotate = true;
                _planet.rotationSpeed = 10;
            }
        }
        
        // If a player exits the boundary, he gets pulled back in to the center.
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Boundary"))
            {
                _planet = other.GetComponentInParent<GravityAttractor>();
            }
            
            /* When a player leaves the ground, his rotation
             is no longer dependent on the planet while gravity
             is still enacted on them. */
            if (other.CompareTag("InnerGravity"))
            {
                _shouldRotate = false;
            }
        }


    }
}
