using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour {
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    private void Start() {
        resolutions = Screen.resolutions;
        
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (var i = 0; i < resolutions.Length; i++) {
            Resolution resolution = resolutions[i];
            string option = $"{resolution.width} x {resolution.height}";
            options.Add(option);

            if (resolution.width == Screen.currentResolution.width &&
                resolution.height == Screen.currentResolution.height) {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex) {
        SoundManager.Instance.PlaySound(SoundManager.Sound.MenuClick);
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume) {
        SoundManager.Instance.PlaySound(SoundManager.Sound.MenuClick);
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIndex) {
        SoundManager.Instance.PlaySound(SoundManager.Sound.MenuClick);
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen) {
        SoundManager.Instance.PlaySound(SoundManager.Sound.MenuClick);
        Screen.fullScreen = isFullScreen;
    }
}
