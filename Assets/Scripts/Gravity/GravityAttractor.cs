using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class GravityAttractor : MonoBehaviour
{
    PlayerMovement playerMovement;
    GravityControl gravityControl;

    [Range(1, 5)]
    private int planetGravity = 1;
    public Quaternion targetRotation;
    public float rotationSpeed = 10;

    Vector3 gravityUp;
    Vector3 localUp;


    //This Script rests on the planet and attracts the player to the center of the planet
    private void Awake()
    {
        //playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        //gravityControl = GameObject.FindGameObjectWithTag("Player").GetComponent<GravityControl>();



    }
    public void Attract(Rigidbody body)
    {
        gravityUp = (body.position - transform.position).normalized;



        // Apply downwards gravity to body
        body.AddForce(gravityUp * planetGravity * -10);




    }
    public void Rotate(Rigidbody body)
    {
        localUp = body.transform.up;

        // body.rotation = Quaternion.FromToRotation(localUp, gravityUp) * body.rotation;



        // Allign body's up axis with the center of planet

        Quaternion startRotation = body.rotation;
        Quaternion endRotation = Quaternion.FromToRotation(localUp, gravityUp) * body.rotation;
        body.rotation = Quaternion.Lerp(startRotation, endRotation, rotationSpeed * Time.deltaTime);


    }

}
