using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Planets;

namespace Player
{
    public class CaptureImageManager : MonoBehaviour
    {
        public GameObject captureImageObject;
        public CapturePoint capturePoint;
        public int spriteIndex = 0;

        void Start()
        {
            spriteIndex = 0;
            captureImageObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
           
                
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("CapturePoint"))
            {
                capturePoint = other.GetComponentInParent<CapturePoint>();

            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("CapturePoint"))
            {
                captureImageObject.SetActive(false);
            }
        }
    }
}
