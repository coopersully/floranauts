using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerActionBar : MonoBehaviour
    {
        public TextMeshProUGUI actionBar;
        public Animator animator;

        private static readonly int SendTrigger = Animator.StringToHash("Send");

        public void Send(string message)
        {
            actionBar.SetText(message);
            animator.SetTrigger(SendTrigger);
        }
    }
}
