using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BlackHole
{
    public class BlackHoleController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            var playerMovement = other.GetComponent<PlayerMovement>();
            var forceVector = new Vector3(AllOrNothing(), 0, AllOrNothing());
            playerMovement.AddForce(forceVector, ForceMode.Impulse);
        }

        private static int AllOrNothing() => Random.Range(0, 1) * 10;
    }
}
