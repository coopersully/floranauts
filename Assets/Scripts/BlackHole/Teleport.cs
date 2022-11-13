using System.Collections;
using UnityEngine;

namespace BlackHole
{
    public class Teleport : MonoBehaviour
    {
        public GameObject portal;
        private Transform playerTransform;
        public GameObject[] teleportPoints;
        private int randNum = 1;


        void Awake()
        {
            // finds all objects with "respawn' tag and adds them to an array
            teleportPoints = GameObject.FindGameObjectsWithTag("BlackHoleSpawn");
            portal = GameObject.FindGameObjectWithTag("Portal");
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("BlackHole"))
            {
                //Teleports player to random teleport point
                randNum = Random.Range(0, teleportPoints.Length);
                gameObject.transform.position = teleportPoints[randNum].transform.position;
                StartCoroutine(OpenPortal());
            }
        }
        IEnumerator OpenPortal()
        {
            portal.SetActive(true);
            portal.transform.position = teleportPoints[randNum].transform.position;

            yield return new WaitForSeconds(3f);
            portal.SetActive(false);
        }
    }
}
