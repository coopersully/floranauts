using Player;
using UnityEngine;

public class Planet: MonoBehaviour
{
    public GameObject[] canvasPrimaries;
    public GameObject[] canvasAccents;

    private Material[] _canvasPrimaries;
    private Material[] _canvasAccents;
   
    public Color unclaimedColor = Color.white;

    private void Awake()
    {
        _canvasPrimaries = new Material[canvasPrimaries.Length];
        _canvasAccents = new Material[canvasAccents.Length];

        for (int i = 0; i < canvasPrimaries.Length; i++)
        {
            _canvasPrimaries[i] = canvasPrimaries[i].GetComponent<Renderer>().material;
        }
        
        for (int i = 0; i < canvasAccents.Length; i++)
        {
            _canvasAccents[i] = canvasAccents[i].GetComponent<Renderer>().material;
        }

        // Randomize rotation
        gameObject.transform.SetPositionAndRotation(
            transform.position,
            new Quaternion(0, Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f)));
    }

    public void Unclaim()
    {
        foreach (var canvasPrimary in _canvasPrimaries)
        {
            canvasPrimary.color = unclaimedColor;
        }
        foreach (var canvasAccent in _canvasAccents)
        {
            canvasAccent.color = unclaimedColor;
        }
    }

    public void Claim(PlayerCapture playerCapture)
    {
        foreach (var canvasPrimary in _canvasPrimaries)
        {
            canvasPrimary.color = playerCapture.primaryColor;
        }
        foreach (var canvasAccent in _canvasAccents)
        {
            canvasAccent.color = playerCapture.accentColor;
        }
    }
  
}