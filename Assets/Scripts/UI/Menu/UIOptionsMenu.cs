using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using AutoBattler.Data.Player;

public class UIOptionsMenu : MonoBehaviour
{
    private static int MIN_VOLUME_VALUE = -60;
    private static int MAX_VOLUME_VALUE = 10;
    private static int MIN_SLIDER_VALUE = 0;
    private static int MAX_SLIDER_VALUE = 100;

    [Header("Components")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private Slider masterVolumeSlider,
        musicVolumeSlider,
        effectsVolumeSlider,
        UIVolumeSlider;
    [SerializeField] private TMP_Text masterVolumeHandleText, 
        musicVolumeHandleText, 
        effectsVolumeHandleText, 
        UIVolumeHandleText;

    [Header("Sounds")]
    [SerializeField] private AudioMixer audioMixer;

    private Resolution[] resolutions;

    private void Start()
    {
        SetResolutionDropdownItems();
    }

    private void OnEnable()
    {
        SetPlayerSettings();
    }

    private void SetResolutionDropdownItems()
    {
        resolutionDropdown.ClearOptions();

        resolutions = Screen.resolutions;

        List<string> items = new List<string>();
        StringBuilder item = new StringBuilder();
        int selectedResolutionIndex = 0;

        foreach (Resolution resolution in resolutions)
        {
            item.Clear();
            item.Append(resolution.width);
            item.Append(" x ");
            item.Append(resolution.height);
            item.Append(" ");
            item.Append(resolution.refreshRate);
            item.Append(" hz");
            items.Add(item.ToString());

            if (Screen.currentResolution.width == resolution.width &&
                Screen.currentResolution.height == resolution.height &&
                Screen.currentResolution.refreshRate == resolution.refreshRate)
            {
                selectedResolutionIndex = items.Count - 1;
            }
        }

        resolutionDropdown.AddOptions(items);
        resolutionDropdown.value = selectedResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private void SetPlayerSettings()
    {
        fullScreenToggle.isOn = PlayerSettings.IsFullScreen;

        masterVolumeSlider.value = PlayerSettings.MasterVolume;
        musicVolumeSlider.value = PlayerSettings.MusicVolume;
        effectsVolumeSlider.value = PlayerSettings.EffectsVolume;
        UIVolumeSlider.value = PlayerSettings.UIVolume;

        masterVolumeHandleText.text = 
            Mathf.Lerp(MIN_SLIDER_VALUE, MAX_SLIDER_VALUE, PlayerSettings.MasterVolume / 100)
            .ToString();
        musicVolumeHandleText.text = 
            Mathf.Lerp(MIN_SLIDER_VALUE, MAX_SLIDER_VALUE, PlayerSettings.MusicVolume / 100)
            .ToString();
        effectsVolumeHandleText.text = 
            Mathf.Lerp(MIN_SLIDER_VALUE, MAX_SLIDER_VALUE, PlayerSettings.EffectsVolume / 100)
            .ToString();
        UIVolumeHandleText.text = 
            Mathf.Lerp(MIN_SLIDER_VALUE, MAX_SLIDER_VALUE, PlayerSettings.UIVolume / 100)
            .ToString();
    }

    public void SetMasterVolume(float volume)
    {
        PlayerSettings.MasterVolume = volume;
        masterVolumeHandleText.text = volume.ToString();
        audioMixer.SetFloat("MasterVolume", Mathf.Lerp(MIN_VOLUME_VALUE, MAX_VOLUME_VALUE, volume / 100));
    }

    public void SetMusicVolume(float volume)
    {
        PlayerSettings.MusicVolume = volume;
        musicVolumeHandleText.text = volume.ToString();
        audioMixer.SetFloat("MusicVolume", Mathf.Lerp(MIN_VOLUME_VALUE, MAX_VOLUME_VALUE, volume / 100));
    }

    public void SetEffectsVolume(float volume)
    {
        PlayerSettings.EffectsVolume = volume;
        effectsVolumeHandleText.text = volume.ToString();
        audioMixer.SetFloat("EffectsVolume", Mathf.Lerp(MIN_VOLUME_VALUE, MAX_VOLUME_VALUE, volume / 100));
    }

    public void SetUIVolume(float volume)
    {
        PlayerSettings.UIVolume = volume;
        UIVolumeHandleText.text = volume.ToString();
        audioMixer.SetFloat("UIVolume", Mathf.Lerp(MIN_VOLUME_VALUE, MAX_VOLUME_VALUE, volume / 100));
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        PlayerSettings.Resolution = resolution;
        Screen.SetResolution(resolution.width, resolution.height, PlayerSettings.IsFullScreen, resolution.refreshRate);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        PlayerSettings.IsFullScreen = isFullScreen;
        Screen.fullScreen = isFullScreen;
    }
}
