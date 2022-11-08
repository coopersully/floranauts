using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    Rigidbody _otherRb;
    Vector3 _direction;
    public float _knockbackForce = 200f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody)
        {
            _otherRb = other.attachedRigidbody;
            _direction = _otherRb.position - transform.position;
            _direction.y = 2;
            _otherRb.AddForce(_direction * _knockbackForce * Time.deltaTime, ForceMode.Impulse);
        }
    }

}
