using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Gravity;

public class RocketProjectile : MonoBehaviour
{
    public Rigidbody _rigidbody;
    private float launchForce = 100;
    public float gravityAttraction = 5;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        //applies forward force to projectile
        _rigidbody.AddForce(transform.forward * launchForce);

    }

    //destroys game object when hits planet
    void OnCollisionEnter(Collision co)
    {
        if (co.gameObject.tag == "Planet")
        {
            Destroy(gameObject);
        }
    }

}
