using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerInput playerInput;
        public GameObject body;

        public void SetBodyActive(bool exists)
        {
            body.SetActive(exists);
            playerInput.camera.gameObject.SetActive(exists);

            if (!exists) playerInput.DeactivateInput();
            else playerInput.ActivateInput();
        }
        
    }
}
