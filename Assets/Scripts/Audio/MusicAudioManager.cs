using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio
{
    public class MusicAudioManager : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip[] songs;

        private void Awake()
        {
            audioSource.clip = songs[Random.Range(0, songs.Length)];
            audioSource.Play();
        }
    }
}