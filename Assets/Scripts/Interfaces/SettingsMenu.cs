using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Interfaces
{
    public class SettingsMenu : MonoBehaviour
    {

        public Slider masterVolume;
        public TextMeshProUGUI masterVolumeValue;
    
        public Slider interfaceVolume;
        public TextMeshProUGUI interfaceVolumeValue;

        public Slider fxVolume;
        public TextMeshProUGUI fxVolumeValue;
        
        public Slider musicVolume;
        public TextMeshProUGUI musicVolumeValue;
    
        private void OnEnable() => RefreshComponents();

        private void RefreshComponents()
        {
            // Master Volume
            masterVolume.value = AudioListener.volume;
            masterVolumeValue.SetText((int) (masterVolume.value * 100.0) + "%");
        
            // UI Volume
            AudioManager.Instance.ui.audioSource.volume = interfaceVolume.value;
            interfaceVolumeValue.SetText((int) (AudioManager.Instance.ui.audioSource.volume * 100.0) + "%");
            
            // FX Volume
            AudioManager.Instance.fx.audioSource.volume = fxVolume.value;
            fxVolumeValue.SetText((int) (AudioManager.Instance.fx.audioSource.volume * 100.0) + "%");
            
            // Music Volume
            AudioManager.Instance.music.audioSource.volume = musicVolume.value;
            musicVolumeValue.SetText((int) (AudioManager.Instance.music.audioSource.volume * 100.0) + "%");
        }

        public void UpdateVolume()
        {
            AudioListener.volume = masterVolume.value;
            masterVolumeValue.SetText((int) (AudioListener.volume * 100.0) + "%");
        }
    
        public void UpdateVolumeUI()
        {
            AudioManager.Instance.ui.audioSource.volume = interfaceVolume.value;
            interfaceVolumeValue.SetText((int) (AudioManager.Instance.ui.audioSource.volume * 100.0) + "%");
        }
        
        public void UpdateVolumeFX()
        {
            AudioManager.Instance.fx.audioSource.volume = fxVolume.value;
            fxVolumeValue.SetText((int) (AudioManager.Instance.fx.audioSource.volume * 100.0) + "%");
        }
        
        public void UpdateVolumeMusic()
        {
            AudioManager.Instance.music.audioSource.volume = musicVolume.value;
            musicVolumeValue.SetText((int) (AudioManager.Instance.music.audioSource.volume * 100.0) + "%");
        }
    }
}
