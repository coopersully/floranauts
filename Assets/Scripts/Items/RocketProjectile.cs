using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Gravity;

public class RocketProjectile : MonoBehaviour
{
    private GravityAttractor _planet;
    public Rigidbody _rigidbody;
    private float launchForce = 500;
    public float gravityAttraction = 5;
    public GameObject[] allPlanets;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        allPlanets = GameObject.FindGameObjectsWithTag("Planet");
        NearestPlanet();

    }

    void Update()
    {
        _planet.Attract(_rigidbody);
        //applies forward force to projectile
       // _rigidbody.AddForce(transform.forward * launchForce);

    }
    void FixedUpdate()
    {
        _planet.Attract(_rigidbody);

        _planet.RotateRocket(_rigidbody);
    }

    //destroys game object when hits planet
    void OnCollisionEnter(Collision co)
    {
        if (co.gameObject.tag != "BlackHole")
        {
            Destroy(gameObject);
            //Instantiate Explosion
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gravity"))
        {
            _planet = other.GetComponentInParent<GravityAttractor>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Boundary"))
        {
            _planet = other.GetComponentInParent<GravityAttractor>();

        }
    }
    private void NearestPlanet()
    {
        //Finds closest planet to player and attracts player
        var closestPlanet = allPlanets[0];
        var distance = Vector3.Distance(transform.position, allPlanets[0].transform.position);

        //goes through array and if the next planet in array is closer, sets that to closest planet
        for (int i = 0; i < allPlanets.Length; i++)
        {
            var tempDistance = Vector3.Distance(transform.position, allPlanets[i].transform.position);
            if (tempDistance < distance )
            {
                closestPlanet = allPlanets[i];
                distance = tempDistance;
            }

        }

        _planet = closestPlanet.GetComponent<GravityAttractor>();

        //need to find a way to exclude the planet player was just on
    }


}
