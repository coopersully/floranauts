using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interfaces
{
    public class PlayerIcon : MonoBehaviour
    {
        public PlayerInput playerInput;
        public Transform target;
        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            SetColor(PlayerColor.GetPrimary(playerInput.playerIndex));

            name = target.name + " (Minimap Marker)";
            transform.SetParent(null);
        }

        private void Update()
        {
            var targetPosition = new Vector3(target.position.x, 50, target.position.z);
            transform.position = targetPosition;
        }

        public void SetColor(Color32 color32) => _meshRenderer.material.color = color32;
    }
}
