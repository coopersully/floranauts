using UnityEngine;
using System.Collections;
 
public class PlanetSpawner : MonoBehaviour
{
    public int numObjects = 12;
    public float radius;
    public GameObject planetPrefab;

    private void Awake()
    {
        var center = transform.position;
        for (var i = 0; i < numObjects; i++)
        {
            var a = i * 30;
            var pos = RandomCircle(center, a);
            var planet = Instantiate(planetPrefab, pos, Quaternion.identity);
            planet.transform.SetParent(gameObject.transform);
        }
    }

    private Vector3 RandomCircle(Vector3 center, int a)
    {
        Debug.Log(a);
        float ang = a;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        return pos;
    }

}