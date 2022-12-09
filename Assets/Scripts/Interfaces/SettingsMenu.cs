using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Interfaces
{
    public class SettingsMenu : MonoBehaviour
    {

        public Slider sliderMaster;
        public TextMeshProUGUI textMaster;
    
        public Slider sliderInterface;
        public TextMeshProUGUI textInterface;

        public Slider sliderSoundFX;
        public TextMeshProUGUI textSoundFX;
        
        public Slider sliderMusic;
        public TextMeshProUGUI textMusic;
    
        private void OnEnable() => RefreshComponents();

        private void RefreshComponents()
        {
            float valueMaster = PlayerPrefs.GetFloat("volumeMaster", 100);
            float valueInterface = PlayerPrefs.GetFloat("volumeInterface", 100);
            float valueSoundFX = PlayerPrefs.GetFloat("volumeSoundFX", 100);
            float valueMusic = PlayerPrefs.GetFloat("volumeMusic", 100);

            // Master Volume
            AudioListener.volume = valueMaster / 100f;
            sliderMaster.SetValueWithoutNotify(AudioListener.volume);
            textMaster.SetText((int) valueMaster + "%");
        
            // Interface Volume
            sliderInterface.SetValueWithoutNotify(valueInterface / 100);
            AudioManager.Instance.ui.audioSource.volume = sliderInterface.value;
            textInterface.SetText((int) valueInterface + "%");
            
            // FX Volume
            sliderSoundFX.SetValueWithoutNotify(valueSoundFX / 100);
            AudioManager.Instance.fx.audioSource.volume = sliderSoundFX.value;
            textSoundFX.SetText((int) valueSoundFX + "%");
            
            // Music Volume
            sliderMusic.SetValueWithoutNotify(valueMusic / 100);
            AudioManager.Instance.music.audioSource.volume = sliderMusic.value;
            textMusic.SetText((int) valueMusic+ "%");
        }

        public void UpdateVolume()
        {
            AudioListener.volume = sliderMaster.value;
            textMaster.SetText((int) (AudioListener.volume * 100.0) + "%");
        }
    
        public void UpdateVolumeUI()
        {
            AudioManager.Instance.ui.audioSource.volume = sliderInterface.value;
            textInterface.SetText((int) (AudioManager.Instance.ui.audioSource.volume * 100.0) + "%");
        }
        
        public void UpdateVolumeFX()
        {
            AudioManager.Instance.fx.audioSource.volume = sliderSoundFX.value;
            textSoundFX.SetText((int) (AudioManager.Instance.fx.audioSource.volume * 100.0) + "%");
        }
        
        public void UpdateVolumeMusic()
        {
            AudioManager.Instance.music.audioSource.volume = sliderMusic.value;
            textMusic.SetText((int) (AudioManager.Instance.music.audioSource.volume * 100.0) + "%");
        }

        public void SaveCurrentPrefs()
        {
            PlayerPrefs.SetFloat("volumeMaster", sliderMaster.value * 100);
            PlayerPrefs.SetFloat("volumeInterface", sliderInterface.value * 100);
            PlayerPrefs.SetFloat("volumeSoundFX", sliderSoundFX.value * 100);
            PlayerPrefs.SetFloat("volumeMusic", sliderMusic.value * 100);
            PlayerPrefs.Save();
        }
    }
}
