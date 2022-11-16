using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerColor : MonoBehaviour
    {
        private PlayerInput _playerInput;
        
        [Header("Per-player Colors")]
        public Color primary;
        public Color secondary;
        
        [Header("Skin Elements")]
        public SkinnedMeshRenderer skinPrimary;
        public SkinnedMeshRenderer skinSecondary;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            if (_playerInput.playerIndex == 0)
            {
                // primary = new Color(57, 161, 243, 100); // Blue
                // secondary = new Color(8, 235, 173, 100);   // Teal
                primary = Color.cyan;
                secondary = Color.green;
            }
            else
            {
                // primary = new Color(231, 89, 78, 100); // Red
                // secondary = new Color(251, 193, 50, 100); // Yellow
                primary = Color.red;
                secondary = Color.yellow;
            }
            
            /* Re-color individual player models to represent
             each player's primary and secondary colors. */
            skinPrimary.material.color = primary;
            skinSecondary.material.color = secondary;
        }
    }
}