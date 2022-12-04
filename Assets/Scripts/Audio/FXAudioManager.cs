using UnityEngine;

namespace Audio
{
    public class FXAudioManager : MonoBehaviour
    {
        public AudioSource audioSource;
    
        public AudioClip rocketLaunch;
        public AudioClip rocketExplode;
        public AudioClip freezeRayLaunch;
        public AudioClip freezeRayHit;
        public AudioClip capturePlanet;
        public AudioClip unCapturePlanet;
        public AudioClip blackHole;

        public void RocketLaunch()
        {
            audioSource.PlayOneShot(rocketLaunch);
        }
        public void RocketExplode()
        {
            audioSource.PlayOneShot(rocketExplode);
        }
        public void FreezeRayLaunch()
        {
            audioSource.PlayOneShot(freezeRayLaunch);
        }
        public void FreezeRayHit()
        {
            audioSource.PlayOneShot(freezeRayHit);
        }
        public void CapturePlanet()
        {
            audioSource.PlayOneShot(capturePlanet);
        }
        public void UnCapturePlanet()
        {
            audioSource.PlayOneShot(unCapturePlanet);
        }
        public void BlackHole()
        {
            audioSource.PlayOneShot(blackHole);
        }
    }
}