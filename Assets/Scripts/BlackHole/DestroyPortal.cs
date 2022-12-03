using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPortal : MonoBehaviour
{
    public float timer = .5f;
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(timer);
        Destroy(this.gameObject);
    }
}
