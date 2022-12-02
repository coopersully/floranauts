using Player;
using UnityEngine;
using UnityEngine.LowLevel;
using System.Collections;


namespace Gravity
{
    // This Script goes on the player and tells the planet what to attract.
    public class GravityControl : MonoBehaviour
    {
        private GravityAttractor _planet;
        private PlayerMovement _playerMovement;
        private Rigidbody _rigidbody;

        public GameObject[] allPlanets;
        public GameObject currPlanet;
        private int randNum = 1;

        private bool _shouldRotate = true;
        
        private void Awake()
        {
            //creates array out of all the planets
            allPlanets = GameObject.FindGameObjectsWithTag("Planet");

            //set first planet to random planet in the array
            RandomPlanet();
           
            
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

            if (_shouldRotate) _planet.RotatePlayer(_rigidbody);
        }
        private void OnTriggerEnter(Collider other)
        {
            /* While player is in the air, if hits trigger
             for other planet, gravity switches and player
             slowly rotates. */
            if (!_playerMovement.isGrounded && other.CompareTag("Gravity"))
            {
                _planet = other.GetComponentInParent<GravityAttractor>();
                _planet.playerRotationSpeed = 3;

                _shouldRotate = true;

                currPlanet = other.transform.parent.gameObject;
            }
            
            /* When Player hits trigger closer to the planet,
             the rotation speed increases, snapping him upright
             in relation to planet. */
            if (other.CompareTag("InnerGravity"))
            {
                _shouldRotate = true;
                _planet.playerRotationSpeed = 8;
            }

            if (other.CompareTag("BlackHole"))
            {
                //resets gravity and has player attracted to random planet
                _shouldRotate = true;
                _planet.playerRotationSpeed = 10;
               RandomPlanet();

            }

            if (other.CompareTag("KnockBack"))
            {
                _shouldRotate = false;
                StartCoroutine(KnockBackTimer());
                
            }
        }
        
        // If a player exits the boundary, he gets pulled back in to the center.
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Boundary"))
            {
                _shouldRotate = false;
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
        private void RandomPlanet()
        {
            //chooses random planet and attracts player to that
            randNum = Random.Range(0, allPlanets.Length);
            _planet = allPlanets[randNum].GetComponent<GravityAttractor>();

        }
        private void NearestPlanet()
        {
            //Finds closest planet to player and attracts player
            var closestPlanet = allPlanets[0];
            var distance = Vector3.Distance(transform.position, allPlanets[0].transform.position);

            //goes through array and if the next planet in array is closer, sets that to closest planet
            for (int i = 0; i < allPlanets.Length; i++)
            {
                var tempDistance = Vector3.Distance(transform.position, allPlanets[i].transform.position);
                if (tempDistance < distance && allPlanets[i] != currPlanet) //sets closest planet but excludes current planet
                {
                    closestPlanet = allPlanets[i];
                    distance = tempDistance;
                }
      
            }

            _planet = closestPlanet.GetComponent<GravityAttractor>();

            //need to find a way to exclude the planet player was just on
        }
        private IEnumerator KnockBackTimer()
        {
            //makes sure player is attracted to planet nearest him when he is done with knockback
            NearestPlanet();
            yield return new WaitForSeconds(.75f);
            NearestPlanet();
        }


    }
}
