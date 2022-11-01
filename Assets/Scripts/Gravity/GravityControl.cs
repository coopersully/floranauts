using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityControl : MonoBehaviour
{
    GravityAttractor planet;
    PlayerMovement playerMovement;
    Rigidbody rb;

    bool shouldRotate = true;


    //this Script goes on the player and tells the planet what to attract
    void Awake()
    {
        planet = GameObject.FindGameObjectWithTag("Planet1").GetComponent<GravityAttractor>();
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();

        // disable rigidbody built in gravity and freezes rotations so planet can add that itself
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {

        // Allow this body to be influenced by planet's gravity
        planet.Attract(rb);
        if (shouldRotate) planet.Rotate(rb);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!playerMovement.isGrounded && other.CompareTag("Gravity")) //While player is in the air, if hits trigger for other planet, gravity switches
        {
            planet = other.GetComponentInParent<GravityAttractor>();
            planet.rotationSpeed = 1;
            shouldRotate = true;



        }
        if (other.CompareTag("InnerGravity"))
        {
            shouldRotate = true;
            planet.rotationSpeed = 10;
        }
    }
    private void OnTriggerExit(Collider other)  //if player exits the boundary, he gets pulled back in to the center
    {
        if (other.CompareTag("Boundary"))
        {
            planet = other.GetComponentInParent<GravityAttractor>();
        }
        if (other.CompareTag("InnerGravity"))
        {
            shouldRotate = false;
        }
    }


}
