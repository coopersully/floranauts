using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Gravity;
using BlackHole;

public class RocketProjectile : MonoBehaviour
{
    private GravityAttractor _planet;
    private PlayerMovement _playerMovement;
    private Teleport _blackHole;
    public Rigidbody _rigidbody;

    public float gravityAttraction = 5;
    public GameObject[] allPlanets;

    public GameObject portal;
    [HideInInspector]
    public GameObject[] teleportPoints;
    [HideInInspector]
    public int _randomInt = 1;

    public GameObject[] allPlayers;
    public GameObject otherPlayer;
    private float distanceToPlayer = 10000f;
    private bool _seePlayer = false;

    public GameObject explosion;
    private int hitCounter = 0; //makes sure rocket does not explode on spawn
    

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        allPlanets = GameObject.FindGameObjectsWithTag("Planet");
        NearestPlanet();
        allPlayers = GameObject.FindGameObjectsWithTag("Player");
        FindOtherPlayer();

        //makes array of teleport points for mini portals
        teleportPoints = GameObject.FindGameObjectsWithTag("BlackHoleSpawn");

    }
    void Update()
    {
        
        distanceToPlayer = Vector3.Distance(this.transform.position, otherPlayer.transform.position);
        //rocket attracts to player if in certain distance
        if (distanceToPlayer < 50f)
        {
            gravityAttraction = 100 - distanceToPlayer; //gradually adjust how fast the rocket attracts to player
            _planet = otherPlayer.GetComponentInChildren<GravityAttractor>();
            _seePlayer = true;
            Debug.Log("sees Player");
        }
        else if (_seePlayer == true)
        {
            _seePlayer = false;
            gravityAttraction = 5f;
            
        }
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
            var explosionSpawn = Instantiate(explosion, transform.position, Quaternion.identity);
        }
   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gravity") && !_seePlayer)
        {
            _planet = other.GetComponentInParent<GravityAttractor>();
        }
        if (other.CompareTag("BlackHole"))
        {
            //teleports rocket
            Debug.Log("gotSucked");
            _randomInt = Random.Range(0, teleportPoints.Length);
            this.gameObject.transform.position = teleportPoints[_randomInt].transform.position;
            OpenPortal(_randomInt);
        }
        if (other.CompareTag("PlayerRocketTrigger")) // explodes if hits near player
        {
            if (hitCounter > 0) // keeps rocket from exploding on spawn
            {
                Destroy(this.gameObject);
                var explosionSpawn = Instantiate(explosion, transform.position, Quaternion.identity);
                Debug.Log("hit");
            }
            hitCounter++;
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
    private void FindOtherPlayer()
    {
        //finds the other player for targeting purposes
        otherPlayer = allPlayers[0];
        var distance1 = Vector3.Distance(transform.position, allPlayers[0].transform.position);
        var distance2 = Vector3.Distance(transform.position, allPlayers[1].transform.position);
        if (distance1 > distance2)
        {
            distanceToPlayer = distance1;
            otherPlayer = allPlayers[0];
        }
        else
        {
            distanceToPlayer = distance2;
            otherPlayer = allPlayers[1];
        }



    }
    public void OpenPortal(int num)
    {
        //Spawns mini portal prefab
        var portalSpawn = Instantiate(portal, transform.position, Quaternion.identity);
    }


}
