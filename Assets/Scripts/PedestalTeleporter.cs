using UnityEngine;

public class PedestalTeleporter : MonoBehaviour
{
    public Transform pedestalOne;
    public Transform pedestalTwo;

    private void Awake()
    {
        //PlayerManager.Instance.playerOne.transform.SetPositionAndRotation(pedestalOne.position, Quaternion.identity);
        //PlayerManager.Instance.playerTwo.transform.SetPositionAndRotation(pedestalTwo.position, Quaternion.identity);
    }
}
