using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Interfaces
{
    public class PlayerCard : MonoBehaviour
    {
        public Image background;
        public TextMeshProUGUI index;
        public TextMeshProUGUI title;
        public TextMeshProUGUI subtitle;
        public Image[] icons;
        public uint uid;
    }
}
