using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerColor : MonoBehaviour
    {
        private PlayerInput _playerInput;

        [Header("Per-player Colors")] public Color primary;
        public Color secondary;

        [Header("Skin Elements")] public SkinnedMeshRenderer skinPrimary;
        public SkinnedMeshRenderer skinSecondary;

        private static readonly Color[] PrimaryColors =
        {
            new Color32(57, 161, 243, 255), // Blue
            new Color32(231, 89, 78, 255), // Red
            Color.green,
            Color.magenta,
            Color.yellow,
            Color.cyan,
            Color.gray,
            Color.white,
            Color.black,
            new(0.5f, 0.5f, 0), // Olive
            new(0.5f, 0, 0.5f), // Purple
            new(0, 0.5f, 0.5f), // Teal
            new(1, 0.5f, 0), // Orange
            new(0, 1, 0.5f), // Spring Green
            new(0.5f, 1, 0), // Lime
            new(0, 0.5f, 1), // Light Blue
            new(0.5f, 0, 1), // Violet
            new(1, 0, 0.5f), // Pink
            new(0.7f, 0.7f, 0.7f), // Light Gray
            new(0.3f, 0.3f, 0.3f) // Dark Gray
        };

        private static readonly Color[] SecondaryColors =
        {
            new Color32(8, 235, 173, 255), // Teal
            new Color32(251, 193, 50, 255), // Yellow
            Color.gray,
            Color.cyan,
            Color.red,
            Color.blue,
            Color.black,
            Color.white,
            Color.green,
            new(1, 0.5f, 1), // Light Pink
            new(1, 1, 0.5f), // Light Yellow
            new(0.5f, 1, 1), // Light Cyan
            new(0.5f, 0.5f, 1), // Light Blue
            new(1, 0.5f, 0.5f), // Light Red
            new(0.5f, 0.5f, 0.5f), // Middle Gray
            new(0.3f, 0.7f, 0.3f), // Soft Green
            new(0.7f, 0.3f, 0.3f), // Soft Red
            new(0.3f, 0.3f, 0.7f), // Soft Blue
            new(0.7f, 0.7f, 0.3f), // Soft Yellow
            new(0.3f, 0.7f, 0.7f) // Soft Cyan
        };


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
            // Will cycle through colors if there are more than the length of the array
            return PrimaryColors[index % PrimaryColors.Length];
        }

        public static Color GetSecondary(int index)
        {
            // Will cycle through colors if there are more than the length of the array
            return SecondaryColors[index % SecondaryColors.Length];
        }
    }
}