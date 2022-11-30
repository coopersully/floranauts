using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPortal : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(.5f);
        Destroy(this.gameObject);
    }
}
