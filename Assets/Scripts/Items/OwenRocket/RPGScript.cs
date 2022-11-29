using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGScript : MonoBehaviour
{
    public Rigidbody rocket;

    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Rigidbody clonedRocket;
            clonedRocket = Instantiate(rocket, transform.position, Quaternion.identity);

            clonedRocket.velocity = transform.TransformDirection(Vector3.forward * 45);
        }
    }
}