using UnityEngine;

namespace Gravity
{
    // This Script rests on the planet and attracts the player to the center of the planet
    public class GravityAttractor : MonoBehaviour
    {
        private PlayerMovement _playerMovement;
        private GravityControl _gravityControl;

        [Range(1, 5)]
        public readonly int planetGravity = 1;
        public Quaternion targetRotation;
        public float rotationSpeed = 10;

        private Vector3 _gravityUp;
        private Vector3 _localUp;
        
        // private void Awake()
        // {
        //     playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        //     gravityControl = GameObject.FindGameObjectWithTag("Player").GetComponent<GravityControl>();
        // }
        
        public void Attract(Rigidbody body)
        {
            _gravityUp = (body.position - transform.position).normalized;
            
            // Apply downwards gravity to body
            body.AddForce(_gravityUp * (planetGravity * -10));
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
    }
}
