using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


[ExecuteInEditMode]
public class MeshSpawnPlanet : MonoBehaviour
{
    public Material crystalMat;
    public Material grassMat;
    public Material greenGlowyMat;
    public Material iceMat;
    public Material lavaMat;
    public Material rootsMat;
    public Material swirlsMat;
    public Material waterMat;
    private Material setMat;
    public GameObject planetMesh;
    private Renderer rend;
    

    public enum Planet_Mats
    {
        crystal,
        grass,
        green,
        ice,
        lava,
        swirl,
        roots,
        water
    }
    public Planet_Mats planetMat;
    
    void Start()
    {
        
        switch (planetMat)
        {
            case Planet_Mats.crystal:
                setMat = crystalMat;
                break;
            case Planet_Mats.grass:
                setMat = grassMat;
                break;
            case Planet_Mats.green:
                setMat = greenGlowyMat;
                break;
            case Planet_Mats.ice:
                setMat = iceMat;
                break;
            case Planet_Mats.lava:
                setMat = lavaMat;
                break;
            case Planet_Mats.swirl:
                setMat = swirlsMat;
                break;
            case Planet_Mats.roots:
                setMat = rootsMat;
                break;
            case Planet_Mats.water:
                setMat = waterMat;
                break;
        }
        rend = planetMesh.GetComponent<Renderer>();
        rend.sharedMaterial = setMat;
        
        //planetMesh.GetComponent<Renderer>().materials[1] = setMat;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
