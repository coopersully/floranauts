using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        transform.Rotate(Random.Range(-10, 10), Random.Range(0, 360), Random.Range(-10, 10));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
