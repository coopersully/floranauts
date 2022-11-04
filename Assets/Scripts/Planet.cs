using Player;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Planet: MonoBehaviour
{
    private Material _material;
    
    public Color unclaimedColor = Color.white;

    private void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;
        
        // Randomize rotation
        gameObject.transform.SetPositionAndRotation(
            transform.position,
            new Quaternion(0, Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f))
        );
    }

    public void Unclaim()
    {
        _material.color = unclaimedColor;
    }

    public void Claim(PlayerCapture playerCapture)
    {
        _material.color = playerCapture.playerColor;
    }
}