using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCull : MonoBehaviour
{
    private int invisibleLayer;
    private int groundLayer;
    // Start is called before the first frame update
    void Start()
    {
        invisibleLayer = LayerMask.NameToLayer("Invisible");
        groundLayer = LayerMask.NameToLayer("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collided");
        if (other.CompareTag("GrassSphere"))
        {
            Debug.Log("found Grass");
            other.gameObject.layer = invisibleLayer;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GrassSphere"))
        {
            other.gameObject.layer = groundLayer;
        }
    }
}
