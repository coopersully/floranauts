using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CaptureTextPrompt : MonoBehaviour
{
    private PlayerInput _playerInput;

    public static string promptName;

    //public TextMeshProUGUI BlueCaptureTextPrompt;
    //public TextMeshProUGUI RedCaptureTextPrompt;

    public void ShowPrompt()
    {
        _playerInput = GetComponent<PlayerInput>();

        if (_playerInput.playerIndex == 0)
        {
            promptName = "BlueCaptureTextPrompt";
        }
        else
        {
            promptName = "RedCaptureTextPrompt";
        }
    }
}
