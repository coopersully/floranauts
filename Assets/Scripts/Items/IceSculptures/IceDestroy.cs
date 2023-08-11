using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;

public class IceDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    public float meltTime = 30f;
    void Awake()
    {
        //AudioManager.Instance.fx.FreezeRayHit();
        
        transform.Rotate(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)); // sets random rotation of ice
        StartCoroutine(Die());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Rocket")) return;
        
        Destroy(this.gameObject);

    }
    private IEnumerator Die()
    {
        yield return new WaitForSeconds(meltTime);
        Destroy(this.gameObject);
    }
}
    
