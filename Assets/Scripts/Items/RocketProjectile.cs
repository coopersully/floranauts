using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Gravity;
using BlackHole;

public class RocketProjectile : MonoBehaviour
{
    private GravityAttractor _planet;
    private Teleport _blackHole;
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
        //_planet.Attract(_rigidbody);
        //applies forward force to projectile
       // _rigidbody.AddForce(transform.forward * launchForce);

    }
    void FixedUpdate()
    {
        _planet.AttractRocket(this._rigidbody);

        _planet.RotateRocket(this._rigidbody);
    }

    //destroys game object when hits planet
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
        if (collision.gameObject.tag == "Planet" || collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);

            //Instantiate Explosion
        }
        else if (collision.gameObject.tag == "BlackHole")
            Destroy(this.gameObject);


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
            Debug.Log("Exit");

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
