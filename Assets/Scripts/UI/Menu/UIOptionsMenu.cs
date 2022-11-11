using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AutoBattler.Data.Player;
using AutoBattler.SaveSystem;
using AutoBattler.Managers;

public class UIOptionsMenu : MonoBehaviour
{
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

    private Resolution[] resolutions;

    private void Start()
    {
        SetResolutionDropdownItems();
    }

    private void OnEnable()
    {
        SetPlayerSettings();
    }

    private void OnDisable()
    {
        FileSaveSystem.SaveSettings();
    }

    private void SetResolutionDropdownItems()
    {
        resolutionDropdown.ClearOptions();

        resolutions = Screen.resolutions;

        List<string> items = new List<string>();
        int selectedResolutionIndex = 0;

        foreach (Resolution resolution in resolutions)
        {
            items.Add(resolution.ToString());

            if (PlayerSettings.Resolution.width == resolution.width &&
                PlayerSettings.Resolution.height == resolution.height &&
                PlayerSettings.Resolution.refreshRate == resolution.refreshRate)
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

        SetVolumeHandleText(masterVolumeHandleText, PlayerSettings.MasterVolume);
        SetVolumeHandleText(musicVolumeHandleText, PlayerSettings.MusicVolume);
        SetVolumeHandleText(effectsVolumeHandleText, PlayerSettings.EffectsVolume);
        SetVolumeHandleText(UIVolumeHandleText, PlayerSettings.UIVolume);

        Screen.fullScreen = PlayerSettings.IsFullScreen;
        SetResolution(PlayerSettings.Resolution, PlayerSettings.IsFullScreen);
    }

    public void SetMasterVolume(float volume)
    {
        PlayerSettings.MasterVolume = volume;
        masterVolumeHandleText.text = volume.ToString();
        AudioManager.Instance.SetVolume("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        PlayerSettings.MusicVolume = volume;
        musicVolumeHandleText.text = volume.ToString();
        AudioManager.Instance.SetVolume("MusicVolume", volume);
    }

    public void SetEffectsVolume(float volume)
    {
        PlayerSettings.EffectsVolume = volume;
        effectsVolumeHandleText.text = volume.ToString();
        AudioManager.Instance.SetVolume("EffectsVolume", volume);
    }

    public void SetUIVolume(float volume)
    {
        PlayerSettings.UIVolume = volume;
        UIVolumeHandleText.text = volume.ToString();
        AudioManager.Instance.SetVolume("UIVolume", volume);
    }

    public void SetResolution(int resolutionIndex)
    {
        PlayerSettings.Resolution = resolutions[resolutionIndex];
        SetResolution(PlayerSettings.Resolution, PlayerSettings.IsFullScreen);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        PlayerSettings.IsFullScreen = isFullScreen;
        Screen.fullScreen = isFullScreen;
    }

    private void SetVolumeHandleText(TMP_Text volumeHandleText, float volume)
    {
        volumeHandleText.text =
            Mathf.Lerp(MIN_SLIDER_VALUE, MAX_SLIDER_VALUE, volume / 100)
            .ToString();
    }

    private void SetResolution(Resolution resolution, bool isFullScreen)
    {
        Screen.SetResolution(
            resolution.width,
            resolution.height,
            isFullScreen,
            resolution.refreshRate
            );
    }
}
