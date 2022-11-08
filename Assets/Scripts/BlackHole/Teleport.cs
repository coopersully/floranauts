using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    
    private Transform playerTransform;
    public GameObject[] teleportPoints;
    private int randNum;
    
   

    void Awake()
    {
        // finds all objects with "respawn' tag and adds them to an array
        teleportPoints = GameObject.FindGameObjectsWithTag("Respawn");

    }

  

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlackHole"))
        {
            //Teleports player to random teleport point
            randNum = Random.Range(0, teleportPoints.Length);
            gameObject.transform.position = teleportPoints[randNum].transform.position;

        }

    }
    
}
