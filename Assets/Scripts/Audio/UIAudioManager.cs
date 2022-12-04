using UnityEngine;

namespace Audio
{
    public class UIAudioManager : MonoBehaviour
    {
        public AudioSource audioSource;
    
        public AudioClip cursorClick01;
        public AudioClip cursorClick02;
        public AudioClip cursorClick03;
        public AudioClip cursorClick04;
        public AudioClip cursorClick05;
    
        public AudioClip cursorSelect01;
        public AudioClip cursorSelect02;
        public AudioClip cursorSelect03;
        public AudioClip cursorSelect04;
        public AudioClip cursorSelect05;
    
        public AudioClip cursorMove01;
        public AudioClip cursorMove02;
        public AudioClip cursorMove03;

        public void Click01()
        {
            audioSource.PlayOneShot(cursorClick01);
        }
    
        public void Click02()
        {
            audioSource.PlayOneShot(cursorClick02);
        }
    
        public void Click03()
        {
            audioSource.PlayOneShot(cursorClick03);
        }
    
        public void Click04()
        {
            audioSource.PlayOneShot(cursorClick04);
        }
    
        public void Click05()
        {
            audioSource.PlayOneShot(cursorClick05);
        }
    
        public void Select01()
        {
            audioSource.PlayOneShot(cursorSelect01);
        }

        public void Select02()
        {
            audioSource.PlayOneShot(cursorSelect02);
        }
    
        public void Select03()
        {
            audioSource.PlayOneShot(cursorSelect03);
        }
    
        public void Select04()
        {
            audioSource.PlayOneShot(cursorSelect04);
        }
    
        public void Select05()
        {
            audioSource.PlayOneShot(cursorSelect05);
        }
    }
}