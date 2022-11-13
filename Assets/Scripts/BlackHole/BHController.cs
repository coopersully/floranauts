using Player;
using UnityEngine;
using Random = UnityEngine.Random;

public class BHController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var playerMovement = other.GetComponent<PlayerMovement>();
        playerMovement.rb.AddForce(new Vector3(AllOrNothing(), 0, AllOrNothing()), ForceMode.Impulse);
    }

    private int AllOrNothing()
    {
        return Random.Range(0, 1) * 10;
    }
}
