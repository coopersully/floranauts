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
            _playerInput = GetComponentInParent<PlayerInput>();
            primary = GetPrimary(_playerInput.playerIndex);
            secondary = GetSecondary(_playerInput.playerIndex);
            
            /* Re-color individual player models to represent
             each player's primary and secondary colors. */
            skinPrimary.material.color = primary;
            skinSecondary.material.color = secondary;
        }

        public static Color GetPrimary(int index)
        {
            if (index == 0) return new Color32(57, 161, 243, 255); // Blue
            return new Color32(231, 89, 78, 255); // Red
        }

        public static Color GetSecondary(int index)
        {
            if (index == 0) return new Color32(8, 235, 173, 255); // Teal
            return new Color32(251, 193, 50, 255); // Yellow
        }
    }
}