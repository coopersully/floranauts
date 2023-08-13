using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFollow : MonoBehaviour
{
    private Light dirLight;
    private GameObject lightObject;
    public GameObject playerMesh;
    private int playerLayer;
    // Start is called before the first frame update
    void Start()
    {
        lightObject = new GameObject("Light Object");
        dirLight = lightObject.AddComponent<Light>();
        lightSettings();
   
        
    }

    // Update is called once per frame
    void Update()
    {
        lightObject.transform.LookAt(transform.position);
    }

    void lightSettings()
    {
        lightObject.transform.position = Vector3.zero;
        dirLight.type = LightType.Directional;
        dirLight.intensity = 0.5f; //Change with more players
        dirLight.shadows = LightShadows.Hard;

        

        playerLayer = playerMesh.layer;
        Debug.Log(playerLayer);
        dirLight.cullingMask = (1 << LayerMask.NameToLayer("Ground")) 
            | (1 << playerLayer);

    }
}
