using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio
{
    public class MusicAudioManager : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip original01;
        public AudioClip original02;
        public AudioClip original03;
        public AudioClip original04;
        public AudioClip original05;
        public AudioClip original06;

        private void Awake()
        {
            switch (Random.Range(1, 5))
            {
                case 1:
                    audioSource.clip = original01;
                    audioSource.Play();
                    break;
                case 2:
                    audioSource.clip = original02;
                    audioSource.Play();
                    break;
                case 3:
                    audioSource.clip = original03;
                    audioSource.Play();
                    break;
                case 4:
                    audioSource.clip = original04;
                    audioSource.Play();
                    break;
                case 5:
                    audioSource.clip = original05;
                    audioSource.Play();
                    break;
            }
        }
    }
}