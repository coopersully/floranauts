using System.Collections;
using UnityEngine;

namespace BlackHole
{
    public class Teleport : MonoBehaviour
    {
        public GameObject portal;
        private Transform _playerTransform;
        public GameObject[] teleportPoints;
        private int _randomInt = 1;


        private void Awake()
        {
            // finds all objects with "respawn' tag and adds them to an array
            teleportPoints = GameObject.FindGameObjectsWithTag("BlackHoleSpawn");
            portal = GameObject.FindGameObjectWithTag("Portal");
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("BlackHole")) return;
            
            // Teleports player to random teleport point
            _randomInt = Random.Range(0, teleportPoints.Length);
            gameObject.transform.position = teleportPoints[_randomInt].transform.position;
            StartCoroutine(OpenPortal());
        }

        private IEnumerator OpenPortal()
        {
            portal.SetActive(true);
            portal.transform.position = teleportPoints[_randomInt].transform.position;

            yield return new WaitForSeconds(3f);
            portal.SetActive(false);
        }
    }
}
