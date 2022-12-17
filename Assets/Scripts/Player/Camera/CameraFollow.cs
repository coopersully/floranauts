using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    public class CameraFollow : MonoBehaviour
    {
        public GameObject target;
        public GameObject player;

        public float smoothSpeed = 0.125f;
        public Vector3 offset;

        private void FixedUpdate()
        {
            Vector3 desiredPosition = target.transform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            

            
            
        }
        private void Update()
        {
            this.transform.LookAt(player.transform.position);

        }
    }
}
