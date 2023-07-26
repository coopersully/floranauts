using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Planets
{
    public class SpawnGrass : MonoBehaviour
    {
        public GameObject planetMesh;
        public GameObject grass;
        public float radius = 8f;
        public int number = 300;
        public bool spawn = false;
        [HideInInspector]
        public Material grassMat;
        public Renderer rend;
        
        
        // Start is called before the first frame update
        void Start()
        {
            FindGrassRenderer();
            rend.enabled = false;
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
            rend.enabled = true;
            Vector3 randomPos = planetMesh.transform.position + Random.onUnitSphere * radius;
            for (int i=0; i <= amount; i++)
            {
                GameObject newGrass = Instantiate(grass, randomPos, Quaternion.identity) as GameObject;
                newGrass.transform.up = (newGrass.transform.position - planetMesh.transform.position).normalized;
                randomPos = planetMesh.transform.position + Random.onUnitSphere * radius;
            }
        }

        private void FindGrassRenderer()
        {
            foreach(Transform child in transform)
            {
                if (child.tag == "InnerGravity")
                {
                    rend = child.GetComponent<MeshRenderer>();
                    break;
                }
                   
            }
        }
    }

}