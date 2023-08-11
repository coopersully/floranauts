using System.Collections;
using Audio;
using UnityEngine;
using Gravity;

namespace BlackHole
{
    public class Teleport : MonoBehaviour
    {
        private GravityControl _gravityControl;
        public GameObject portal;
        [HideInInspector] public GameObject[] teleportPoints;
        [HideInInspector] public int randomPointIndex;


        private void Awake()
        {
            // Finds all objects with "respawn' tag and adds them to an array
            
            teleportPoints = GameObject.FindGameObjectsWithTag("BlackHoleSpawn");
            
            randomPointIndex = Random.Range(0, teleportPoints.Length);
            gameObject.transform.position = teleportPoints[randomPointIndex].transform.position;
            StartCoroutine(OpenPortal(randomPointIndex));
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("BlackHole")) return;
            //AudioManager.Instance.fx.BlackHole();
            teleportPoints = GameObject.FindGameObjectsWithTag("BlackHoleSpawn");
            

            // Teleports player to random teleport point
            randomPointIndex = Random.Range(0, teleportPoints.Length);
            this.gameObject.transform.position = teleportPoints[randomPointIndex].transform.position;
            StartCoroutine(OpenPortal(randomPointIndex));
        }


        private IEnumerator OpenPortal(int num)
        {
            var newPortal = Instantiate(portal, teleportPoints[num].transform.position, 
                Quaternion.identity) as GameObject;
      
            yield return new WaitForSeconds(3f);
           // Destroy(newPortal);
        }
    }
}
