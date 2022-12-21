using System.Collections;
using Audio;
using UnityEngine;
using Gravity;

namespace BlackHole
{
    public class Teleport : MonoBehaviour
    {
        private GravityControl gravityControl;
        private GameObject portal;
        [HideInInspector]
        public GameObject[] teleportPoints;
        [HideInInspector]
        public int _randomInt = 1;


        private void Awake()
        {
            // finds all objects with "respawn' tag and adds them to an array
            teleportPoints = GameObject.FindGameObjectsWithTag("BlackHoleSpawn");
            portal = GameObject.FindGameObjectWithTag("Portal");
            _randomInt = Random.Range(0, teleportPoints.Length);
            this.gameObject.transform.position = teleportPoints[_randomInt].transform.position;
            StartCoroutine(OpenPortal(_randomInt));
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("BlackHole")) return;
            //gravityControl.allPlanets = GameObject.FindGameObjectsWithTag("Planet");
            AudioManager.Instance.fx.BlackHole();

            // Teleports player to random teleport point
            _randomInt = Random.Range(0, teleportPoints.Length);
            this.gameObject.transform.position = teleportPoints[_randomInt].transform.position;
            StartCoroutine(OpenPortal(_randomInt));
        }
       

        public IEnumerator OpenPortal(int num)
        {
            
            portal.SetActive(true);
            portal.transform.position = teleportPoints[num].transform.position;

            yield return new WaitForSeconds(3f);
            portal.SetActive(false);
        }
    }
}
