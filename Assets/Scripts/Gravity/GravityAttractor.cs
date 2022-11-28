using Player;
using System.Collections;
using UnityEngine;

namespace Gravity
{
    // This Script rests on the planet and attracts the player to the center of the planet
    public class GravityAttractor : MonoBehaviour
    {
        private PlayerMovement _playerMovement;
        private GravityControl _gravityControl;
        private RocketProjectile _rocketProjectile;

        [Range(1, 10)] public int planetGravity = 1;
        public Quaternion targetRotation;
        [HideInInspector]
        public float rotationSpeed;

        private Vector3 _gravityUp;
        private Vector3 _localUp;
        
        // private void Awake()
        // {
        //     playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        //     gravityControl = GameObject.FindGameObjectWithTag("Player").GetComponent<GravityControl>();
        // }
        
        public void Attract(Rigidbody body)
        {
            StartCoroutine(RotationSpeed());
            _gravityUp = (body.position - transform.position).normalized;

            _playerMovement = body.GetComponentInParent<PlayerMovement>();
            // Apply downwards gravity to body
            body.AddForce(_gravityUp * (planetGravity * _playerMovement.playerGravity));

        }
        
        public void Rotate(Rigidbody body)
        {
            _localUp = body.transform.up;

            // body.rotation = Quaternion.FromToRotation(localUp, gravityUp) * body.rotation;

            // Align body's up axis with the center of planet
            var startRotation = body.rotation;
            var endRotation = Quaternion.FromToRotation(_localUp, _gravityUp) * body.rotation;
            body.rotation = Quaternion.Lerp(startRotation, endRotation, rotationSpeed * Time.deltaTime);
        }
        
        //gradually increases rotation speed for smoother transition between planets
        IEnumerator RotationSpeed()
        {
            while(rotationSpeed <= 8)
            {
                yield return new WaitForSeconds(.2f);
                rotationSpeed += .25f;
            }
        }

        public void ProjectileAttract(Rigidbody body)
        {
            _gravityUp = (body.position - transform.position).normalized;
            body.AddForce(_gravityUp * (planetGravity * _rocketProjectile.gravityAttraction));


        }
    }
}
