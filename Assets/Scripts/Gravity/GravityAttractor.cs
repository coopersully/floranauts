using Player;
using System.Collections;
using UnityEngine;

namespace Gravity
{
    // This Script rests on the planet and attracts the player to the center of the planet
    public class GravityAttractor : MonoBehaviour
    {
        //private PlayerMovement _playerMovement;
        private MoveCineMachine _playerMovement;
        private GravityControl _gravityControl;
        private RocketProjectile _rocketProjectile;
        

        [Range(1, 100)] public int planetGravity = 1;
        public Quaternion targetRotation;
        [HideInInspector]
        public float playerRotationSpeed;

        private Vector3 _gravityUp;
        
        // private void Awake()
        // {
        //     playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        //     gravityControl = GameObject.FindGameObjectWithTag("Player").GetComponent<GravityControl>();
        // }
        
        public void Attract(Rigidbody body)
        {
            StartCoroutine(RotationSpeed());
            _gravityUp = (body.position - transform.position).normalized;

            //_playerMovement = body.GetComponentInParent<PlayerMovement>();
            _playerMovement = body.GetComponentInParent<MoveCineMachine>();

            // Apply downwards gravity to body
            body.AddForce(_gravityUp * (planetGravity * _playerMovement.playerGravity));
            //Debug.Log("Attracting");

        }
       

        public void RotateFollowTarget(GameObject target)
        {
           var localforward = target.transform.forward;

            // body.rotation = Quaternion.FromToRotation(localUp, gravityUp) * body.rotation;

            // Align body's up axis with the center of planet
            var startRotation = target.transform.rotation;
            var endRotation = Quaternion.FromToRotation(localforward, _gravityUp) * target.transform.rotation;
            if (_playerMovement.isGrounded && _playerMovement.isSprinting)
                target.transform.rotation = Quaternion.Lerp(startRotation, endRotation, playerRotationSpeed * 10 * Time.deltaTime);
            else
                target.transform.rotation = Quaternion.Lerp(startRotation, endRotation, playerRotationSpeed * Time.deltaTime);
        }
        public void RotatePlayer(Rigidbody body)
        {
            var localUp = body.transform.up;

            // body.rotation = Quaternion.FromToRotation(localUp, gravityUp) * body.rotation;

            // Align body's up axis with the center of planet
            var startRotation = body.rotation;
            var endRotation = Quaternion.FromToRotation(localUp, _gravityUp) * body.rotation;
            if (_playerMovement.isGrounded && _playerMovement.isSprinting)
                body.rotation = Quaternion.Lerp(startRotation, endRotation, playerRotationSpeed * 10 * Time.deltaTime);
            else
                body.rotation = Quaternion.Lerp(startRotation, endRotation, playerRotationSpeed * Time.deltaTime);
        }

        //gradually increases rotation speed for smoother transition between planets
        IEnumerator RotationSpeed()
        {
            while(playerRotationSpeed <= 8)
            {
                yield return new WaitForSeconds(.2f);
                playerRotationSpeed += .25f;
            }
        }
        
        public void AttractRocket(Rigidbody body)
        {
            StartCoroutine(RotationSpeed());
            _gravityUp = (body.position - transform.position).normalized;

            // Apply downwards gravity to body
            body.AddForce(_gravityUp * (planetGravity * -10));

        }


        public void ProjectileAttract(Rigidbody body)
        {
            _gravityUp = (body.position - transform.position).normalized;
            body.AddForce(_gravityUp * (planetGravity * _rocketProjectile.gravityAttraction));


        }
    }
}
