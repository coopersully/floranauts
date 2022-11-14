using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Interfaces.Popup
{
    public class Popup : MonoBehaviour
    {
        
        public TextMeshProUGUI title;
        public TextMeshProUGUI description;
        public Button cancel;
        public Button proceed;

        private Animator _animator;
        private static readonly int Exit = Animator.StringToHash("exit");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void CancelButton()
        {
            _animator.SetTrigger(Exit);
        }
    }
}
