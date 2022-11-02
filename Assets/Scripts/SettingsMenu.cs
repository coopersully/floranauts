using System;
using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    public Slider masterVolume;
    public TextMeshProUGUI masterVolumeValue;
    
    public Slider interfaceVolume;
    public TextMeshProUGUI interfaceVolumeValue;
    
    private void OnEnable() => RefreshComponents();

    private void RefreshComponents()
    {
        // Master Volume
        masterVolume.value = AudioListener.volume;
        masterVolumeValue.SetText((int) (AudioListener.volume * 100.0) + "%");
        
        // UI Volume
        AudioManager.Instance.ui.audioSource.volume = interfaceVolume.value;
        interfaceVolumeValue.SetText((int) (AudioManager.Instance.ui.audioSource.volume * 100.0) + "%");
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
}
