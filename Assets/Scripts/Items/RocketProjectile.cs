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

    public float gravityAttraction = 5;
    public GameObject[] allPlanets;

    public GameObject portal;
    [HideInInspector]
    public GameObject[] teleportPoints;
    [HideInInspector]
    public int _randomInt = 1;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        allPlanets = GameObject.FindGameObjectsWithTag("Planet");
        NearestPlanet();

        //makes array of teleport points for mini portals
        teleportPoints = GameObject.FindGameObjectsWithTag("BlackHoleSpawn");

    }

    void FixedUpdate()
    {
        _planet.AttractRocket(this._rigidbody);
        _planet.RotateRocket(this._rigidbody);
    }

    //destroys game object when hits planet
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Planet" || collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);

            //Instantiate Explosion
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gravity"))
        {
            _planet = other.GetComponentInParent<GravityAttractor>();
        }
        if (other.CompareTag("BlackHole"))
        {
            //teleports rocket
            Debug.Log("gotSucked");
            _randomInt = Random.Range(0, teleportPoints.Length);
            this.gameObject.transform.position = teleportPoints[_randomInt].transform.position;
            StartCoroutine(OpenPortal(_randomInt));
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
        //Finds closest planet to rocket and attracts
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
    }
    public IEnumerator OpenPortal(int num)
    {
        //Spawns mini portal prefab
        var portalSpawn = Instantiate(portal, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(.5f);
    }


}
