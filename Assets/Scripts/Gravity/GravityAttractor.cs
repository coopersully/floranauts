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

        [Range(1, 10)]
        public int planetGravity = 1;
        private int playerGravity = -25;
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
            
            // Apply downwards gravity to body
            body.AddForce(_gravityUp * (planetGravity * playerGravity));

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
            while(rotationSpeed < 10)
            {
                yield return new WaitForSeconds(.5f);
                rotationSpeed += 3f;
            }
        }
    }
}
