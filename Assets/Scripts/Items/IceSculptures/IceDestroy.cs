using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    public float meltTime = 30f;
    void Awake()
    {
        StartCoroutine(Die());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Rocket"))
            Destroy(this.gameObject);
    }
    private IEnumerator Die()
    {
        yield return new WaitForSeconds(meltTime);
        Destroy(this.gameObject);
    }
}
