using Player;
using UnityEngine;

namespace Planets
{
    public class Planet : MonoBehaviour
    {
        
        [Header("Colors")]
        public Color unclaimedColor = new(147, 147, 147);
        
        [Header("Color Components")]
        public GameObject[] canvasPrimaries;
        public GameObject[] canvasAccents;

        private Material[] _canvasPrimaries;
        private Material[] _canvasAccents;

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
                canvasPrimary.color = playerCapture.color.primary;
            }
            foreach (var canvasAccent in _canvasAccents)
            {
                canvasAccent.color = playerCapture.color.primary;
            }
        }
  
    }
}