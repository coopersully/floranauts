using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        public AudioSource defaultAudioSource;
    
        [Header("Audio Sections")]
        public UIAudioManager ui;
        public FXAudioManager fx;
        public MusicAudioManager music;

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }
    }
}
