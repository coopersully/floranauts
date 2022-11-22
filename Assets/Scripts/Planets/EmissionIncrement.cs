using System.Collections;
using UnityEngine;

namespace Planets
{
    public class EmissionIncrement : MonoBehaviour
    {
        public float fillrate = -1.5f;

        public Material fillmaterial;

        float capturerate = 0.1f;
        private static readonly int Fill = Shader.PropertyToID("_Fill");


        // Start is called before the first frame update
        void Start()
        {
            fillmaterial = new Material(Shader.Find("Shader Graphs/EmissionShader"));
            gameObject.GetComponent<Renderer>().material = fillmaterial;
            fillmaterial.SetFloat(Fill, fillrate);
        }

        IEnumerator Colorfill()
        {
            fillrate = -1.5f;
            while (fillrate < 1.5f)
            {
                fillrate += capturerate;
                fillmaterial.SetFloat(Fill, fillrate);
                yield return new WaitForSeconds(.1f);
            }
        }



        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Colorfill());
            }
        }
    }
}
