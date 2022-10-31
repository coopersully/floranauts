using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAttractor : MonoBehaviour
{
    PlayerMovement playerMovement;
    GravityControl gravityControl;

    [Range(1, 200)]
    public int planetGravity = 10;
    public Quaternion targetRotation;
    //private float RotationSpeed = 20;

    //This Script rests on the planet and attracts the player to the center of the planet
    private void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        gravityControl = GameObject.FindGameObjectWithTag("Player").GetComponent<GravityControl>();

    }
    public void Attract(Rigidbody body)
    {
        Vector3 gravityUp = (body.position - transform.position).normalized;
        Vector3 localUp = body.transform.up;



        // Apply downwards gravity to body
        body.AddForce(gravityUp * -planetGravity);

        // Allign body's up axis with the center of planet
        body.rotation = Quaternion.FromToRotation(localUp, gravityUp) * body.rotation;
        //body.transform.up = Vector3.Lerp(transform.up, gravityUp, RotationSpeed * Time.deltaTime);



        






    }
}
