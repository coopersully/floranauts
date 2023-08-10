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
        private Material grassBaseMat;
        public Color[] colors;
        public Color color;
        private float transparency;
        public float dissolveRate = 1;

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
            float num = 8f;
            grassSphere.transform.localScale = new Vector3(num, num, num);
            grassSphere.SetActive(true);

            switch (grassMat)
            {
                case Grass_Mats.blue:
                    setMat = mats[0];
                    color = colors[0];
                    break;
                case Grass_Mats.red:
                    setMat = mats[1];
                    color = colors[1];
                    break;
                case Grass_Mats.green:
                    setMat = mats[2];
                    color = colors[2];
                    break;
                case Grass_Mats.purple:
                    setMat = mats[3];
                    color = colors[3];
                    break;
            }
            grassBaseMat.color = color;
            grassRenderer.material = setMat;
            grassRenderer.OnEnable();
            StartCoroutine(grassDissolve(amount, num));

        }
        
        IEnumerator grassDissolve(int amount, float num)
        {
            transparency = 1f;
            while(transparency > Random.Range(0.1f, 0.3f))
            {
                transparency -= 0.02f * dissolveRate;
                num = 8f + (1 - transparency);
                grassSphere.transform.localScale = new Vector3(num,num,num);
                grassBaseMat.SetFloat("_Transparency", transparency);
                yield return new WaitForSeconds(.1f);
            }

            Vector3 randomPos = planetMesh.transform.position + Random.onUnitSphere * radius;
            for (int i = 0; i <= amount; i++)
            {
                GameObject newGrass = Instantiate(grass, randomPos, Quaternion.identity) as GameObject;
                newGrass.transform.up = (newGrass.transform.position - planetMesh.transform.position).normalized;
                randomPos = planetMesh.transform.position + Random.onUnitSphere * radius;
                yield return new WaitForSeconds(.3f - (.1f * dissolveRate));
            }

        }
        private void FindGrassSphere()
        {
            grassBaseMat = planetMesh.GetComponent<Renderer>().materials[1];
            grassBaseMat.SetFloat("_OffsetX", Random.Range(0, 100));
            grassBaseMat.SetFloat("_OffsetY", Random.Range(0, 100));
            grassBaseMat.SetFloat("_NoiseSize", Random.Range(75, 200));

            foreach (Transform child in transform)
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