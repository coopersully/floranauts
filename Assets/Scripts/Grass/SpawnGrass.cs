using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Planets
{
    public class SpawnGrass : MonoBehaviour
    {
        private ProceduralGrassRenderer grassRenderer;
        public GameObject planetMesh;
        public GameObject grass;
        public float radius = 8f;
        public int number = 300;
        public bool spawn = false;
        private GameObject grassSphere;

        public Material[] mats;
        public enum Grass_Mats
        {
            blue,
            red,
            green,
            purple
        }
        public Grass_Mats grassMat;
        private Material setMat;
        
        
        // Start is called before the first frame update
        void Start()
        {
            FindGrassSphere();
            grassSphere.SetActive(false);

            radius *= transform.localScale.x;
            int difference = Mathf.RoundToInt(transform.localScale.x);
            if (difference != 0) number *= difference;

            if(spawn) spawnGrass(number);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void spawnGrass(int amount)
        {
            switch (grassMat)
            {
                case Grass_Mats.blue:
                    setMat = mats[0];
                    break;
                case Grass_Mats.red:
                    setMat = mats[1];
                    break;
                case Grass_Mats.green:
                    setMat = mats[2];
                    break;
                case Grass_Mats.purple:
                    setMat = mats[3];
                    break;
            }
            grassRenderer.material = setMat;
            grassSphere.SetActive(true);

            Vector3 randomPos = planetMesh.transform.position + Random.onUnitSphere * radius;
            for (int i=0; i <= amount; i++)
            {
                GameObject newGrass = Instantiate(grass, randomPos, Quaternion.identity) as GameObject;
                newGrass.transform.up = (newGrass.transform.position - planetMesh.transform.position).normalized;
                randomPos = planetMesh.transform.position + Random.onUnitSphere * radius;
            }
        }

        private void FindGrassSphere()
        {
            
            foreach(Transform child in transform)
            {
                if (child.tag == "GrassSphere")
                {
                    grassSphere = child.gameObject;
                    grassRenderer = grassSphere.GetComponent<ProceduralGrassRenderer>();

                    break;
                }
                   
            }
        }
    }

}