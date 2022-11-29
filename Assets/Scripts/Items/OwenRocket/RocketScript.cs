using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour
{
    GameObject explosion;

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        Instantiate(explosion, transform.position, Quaternion.identity);
    }
}