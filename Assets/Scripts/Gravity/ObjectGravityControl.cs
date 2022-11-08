using Player;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Gravity
{
    // This Script goes on objects so they are also attracted by planet Gravity
    public class ObjectGravityControl : MonoBehaviour
    {
        private GravityAttractor _planet;
        private Rigidbody _rigidbody;

        public GameObject[] allPlanets;
        private int randNum;

        private bool _shouldRotate = true;

        private void Awake()
        {
            //creates array out of all the planets
            allPlanets = GameObject.FindGameObjectsWithTag("Planet");
            randNum = Random.Range(0, allPlanets.Length);

            //set first planet to random planet in the array
            randNum = Random.Range(0, allPlanets.Length);
            _planet = allPlanets[randNum].GetComponent<GravityAttractor>();
            _planet = GetComponentInParent<GravityAttractor>();



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
            /* if hits trigger
             for other planet, gravity switches and player
             slowly rotates. */
            if (other.CompareTag("Gravity"))
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

            if (other.CompareTag("BlackHole"))
            {
                //resets gravity and has player attracted to random planet
                _shouldRotate = true;
                _planet.rotationSpeed = 10;
                randNum = Random.Range(0, allPlanets.Length);
                _planet = allPlanets[randNum].GetComponent<GravityAttractor>();

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
