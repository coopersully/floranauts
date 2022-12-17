using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Interfaces
{
    public class SettingsMenu : MonoBehaviour
    {
        
        [Header("Volume")]
        public Slider sliderMaster;
        public TextMeshProUGUI textMaster;
    
        public Slider sliderInterface;
        public TextMeshProUGUI textInterface;

        public Slider sliderSoundFX;
        public TextMeshProUGUI textSoundFX;
        
        public Slider sliderMusic;
        public TextMeshProUGUI textMusic;
        
        [Header("Sensitivity")]
        public Slider sliderMouse;
        public TextMeshProUGUI textMouse;
        
        public Slider sliderController;
        public TextMeshProUGUI textController;
    
        private void OnEnable() => RefreshComponents();

        public void RefreshComponents()
        {
            RefreshComponentsVolume();
            RefreshComponentsSensitivity();
        }

        #region Volume

        public void RefreshComponentsVolume()
        {
            float valueMaster = PlayerPrefs.GetFloat("volumeMaster", 100);
            float valueInterface = PlayerPrefs.GetFloat("volumeInterface", 100);
            float valueSoundFX = PlayerPrefs.GetFloat("volumeSoundFX", 100);
            float valueMusic = PlayerPrefs.GetFloat("volumeMusic", 100);

            // Master Volume
            AudioListener.volume = valueMaster / 100f;
            sliderMaster.SetValueWithoutNotify(AudioListener.volume * 100);
            textMaster.SetText((int) valueMaster + "%");
        
            // Interface Volume
            sliderInterface.SetValueWithoutNotify(valueInterface);
            AudioManager.Instance.ui.audioSource.volume = sliderInterface.value / 100f;
            textInterface.SetText((int) valueInterface + "%");
            
            // Effects Volume
            sliderSoundFX.SetValueWithoutNotify(valueSoundFX);
            AudioManager.Instance.fx.audioSource.volume = sliderSoundFX.value / 100f;
            textSoundFX.SetText((int) valueSoundFX + "%");
            
            // Music Volume
            sliderMusic.SetValueWithoutNotify(valueMusic);
            AudioManager.Instance.music.audioSource.volume = sliderMusic.value / 100f;
            textMusic.SetText((int) valueMusic+ "%");
        }

        public void OnUpdateVolume()
        {
            AudioListener.volume = sliderMaster.value / 100f;
            textMaster.SetText((int) (AudioListener.volume * 100.0) + "%");
        }
    
        public void OnUpdateVolumeUI()
        {
            AudioManager.Instance.ui.audioSource.volume = sliderInterface.value / 100f;
            textInterface.SetText((int) (AudioManager.Instance.ui.audioSource.volume * 100.0) + "%");
        }
        
        public void OnUpdateVolumeFX()
        {
            AudioManager.Instance.fx.audioSource.volume = sliderSoundFX.value / 100f;
            textSoundFX.SetText((int) (AudioManager.Instance.fx.audioSource.volume * 100.0) + "%");
        }
        
        public void OnUpdateVolumeMusic()
        {
            AudioManager.Instance.music.audioSource.volume = sliderMusic.value / 100f;
            textMusic.SetText((int) (AudioManager.Instance.music.audioSource.volume * 100.0) + "%");
        }

        public void SavePrefsVolume()
        {
            PlayerPrefs.SetFloat("volumeMaster", sliderMaster.value);
            PlayerPrefs.SetFloat("volumeInterface", sliderInterface.value);
            PlayerPrefs.SetFloat("volumeSoundFX", sliderSoundFX.value);
            PlayerPrefs.SetFloat("volumeMusic", sliderMusic.value);
            PlayerPrefs.Save();
        }

        #endregion

        #region Sensitivity
        
        public void RefreshComponentsSensitivity()
        {
            float valueMouseSens = PlayerPrefs.GetFloat("sensMouse", 1);
            float valueControllerSens = PlayerPrefs.GetFloat("sensController", 1);
            
            // Mouse sensitivity
            sliderMouse.SetValueWithoutNotify(valueMouseSens);
            textMouse.SetText(valueMouseSens.ToString("0.00"));
            
            // Controller sensitivity
            sliderController.SetValueWithoutNotify(valueControllerSens);
            textController.SetText(valueControllerSens.ToString("0.00"));
        }

        public void OnUpdateSensitivityMouse()
        {
            textMouse.SetText(sliderMouse.value.ToString("0.00"));
        }
        
        public void OnUpdateSensitivityController()
        {
            textController.SetText(sliderController.value.ToString("0.00"));
        }
        
        public void SavePrefsSensitivity()
        {
            PlayerPrefs.SetFloat("sensMouse", sliderMouse.value);
            PlayerPrefs.SetFloat("sensController", sliderController.value);
            PlayerPrefs.Save();
        }

        #endregion
        
        public void SavePrefsAll()
        {
            SavePrefsVolume();
            SavePrefsSensitivity();
        }
    }
}
