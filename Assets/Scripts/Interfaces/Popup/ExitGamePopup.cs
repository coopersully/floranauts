using UnityEngine;

namespace Interfaces.Popup
{
    public class ExitGamePopup : MonoBehaviour
    {
        public void Proceed()
        {
            Application.Quit();
        }
    }
}