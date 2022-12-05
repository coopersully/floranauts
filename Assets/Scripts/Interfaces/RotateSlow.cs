using UnityEngine;

namespace Interfaces
{
    public class RotateSlow : MonoBehaviour
    {
        public float amount;
        private void Update() => gameObject.transform.Rotate(0, amount, 0);
    }
}
