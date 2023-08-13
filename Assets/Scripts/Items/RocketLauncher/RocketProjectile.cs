using System.Collections;
using System.Collections.Generic;
using Audio;
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
    public GameObject samePlayer;
    private float distanceToPlayer = 10000f;
    private bool _seePlayer = false;
    public float _seeDistance = 50f;

    public GameObject explosion;
    private int hitCounter = 0; //makes sure rocket does not explode on spawn
    public bool targetFound = false;


    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        allPlanets = GameObject.FindGameObjectsWithTag("Planet");
        NearestPlanet();
        allPlayers = GameObject.FindGameObjectsWithTag("Player");
        FindOtherPlayer();

        this.transform.forward = samePlayer.transform.forward; // sets starting rotation to same as player

        //makes array of teleport points for mini portals
        teleportPoints = GameObject.FindGameObjectsWithTag("BlackHoleSpawn");
        StartCoroutine(Rotate());

    }
    void Update()
    {
        if (otherPlayer == null)
            Debug.Log("Null");
        distanceToPlayer = Vector3.Distance(this.transform.position, otherPlayer.transform.position);
        //rocket attracts to player if in certain distance
        if (distanceToPlayer <= _seeDistance)
        {
            gravityAttraction = (_seeDistance * 2) - distanceToPlayer; //gradually adjust how fast the rocket attracts to player
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

    private void FixedUpdate() => _planet.AttractProjectile(_rigidbody, targetFound); 
       
    

    //destroys game object when hits planet
    private void OnCollisionEnter(Collision collision)
    {

        Destroy(this.gameObject);
        if(this.gameObject.tag == "Rocket")
        {
            //AudioManager.Instance.fx.RocketExplode();
        }

        Instantiate(explosion, transform.position, Quaternion.identity);




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
                AudioManager.Instance.fx.RocketExplode();
                Instantiate(explosion, transform.position, Quaternion.identity);
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
        samePlayer = allPlayers[0];
        var distance1 = Vector3.Distance(transform.position, allPlayers[0].transform.position);
        var distance2 = Vector3.Distance(transform.position, allPlayers[1].transform.position);
        if (distance1 > distance2)
        {
            distanceToPlayer = distance1;
            otherPlayer = allPlayers[0];
            samePlayer = allPlayers[1];

        }
        else
        {
            distanceToPlayer = distance2;
            otherPlayer = allPlayers[1];
            samePlayer = allPlayers[0];

        }



    }
    public void OpenPortal(int num)
    {
        //Spawns mini portal prefab
        var portalSpawn = Instantiate(portal, transform.position, Quaternion.identity);
    }
    private IEnumerator Rotate()
    {
        //projectile will always point forward
        while (this.gameObject.activeSelf)
        {
            var oldPosition = this.transform.position;
            yield return new WaitForSeconds(.1f);
            var newPosition = this.transform.position;
            this.transform.forward = newPosition - oldPosition;
        }
    }

}
