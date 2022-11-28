using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AutoBattler.SaveSystem.Data;
using AutoBattler.SaveSystem;
using AutoBattler.Managers;
using AutoBattler.Data.ScriptableObjects.Characteristics;

public class UIOptionsMenu : MonoBehaviour
{
    private static int MIN_VOLUME_SLIDER_VALUE = 0;
    private static int MAX_VOLUME_SLIDER_VALUE = 100;

    [Header("Characteristics")]
    [SerializeField] private MemberCharacteristics playerCharacteristics;

    [Header("Pages")]
    [SerializeField] private List<GameObject> pages;

    [Header("Components")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullScreenToggle,
        muteOtherFieldsToggle;
    [SerializeField] private Slider masterVolumeSlider,
        musicVolumeSlider,
        effectsVolumeSlider,
        UIVolumeSlider,
        startHealthAmountSlider,
        maxGoldenCupAmountSlider;
    [SerializeField] private TMP_Text masterVolumeHandleText,
        musicVolumeHandleText,
        effectsVolumeHandleText,
        UIVolumeHandleText,
        startHealthAmountHandleText,
        maxGoldenCupAmountHandleText;

    private int currentPageIndex;
    private Resolution[] resolutions;

    private void Start()
    {
        SetResolutionDropdownItems();
        currentPageIndex = 0;
        GoToPage(currentPageIndex);
    }

    private void OnEnable()
    {
        SetPlayerSettings();
    }

    private void OnDisable()
    {
        FileSaveSystem.SaveSettings();
    }

    public void GoToNextPage() => GoToPage(currentPageIndex + 1);

    public void GoToPreviousPage() => GoToPage(currentPageIndex - 1);

    public void GoToPage(int index)
    {
        if (index < 0 || index >= pages.Count)
            return;

        pages[currentPageIndex].gameObject.SetActive(false);
        pages[index].gameObject.SetActive(true);

        currentPageIndex = index;
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
        PlayerSettings.StartHealthAmount = 
            PlayerSettings.StartHealthAmount <= 0 ? 
            playerCharacteristics.StartHealth : 
            PlayerSettings.StartHealthAmount;

        PlayerSettings.MaxGoldenCupAmount =
            PlayerSettings.MaxGoldenCupAmount <= 0 ?
            playerCharacteristics.MaxGoldenCupAmount :
            PlayerSettings.MaxGoldenCupAmount;

        fullScreenToggle.isOn = PlayerSettings.IsFullScreen;
        muteOtherFieldsToggle.isOn = PlayerSettings.IsMuteOtherFields;

        masterVolumeSlider.value = PlayerSettings.MasterVolume;
        musicVolumeSlider.value = PlayerSettings.MusicVolume;
        effectsVolumeSlider.value = PlayerSettings.EffectsVolume;
        UIVolumeSlider.value = PlayerSettings.UIVolume;
        startHealthAmountSlider.value = PlayerSettings.StartHealthAmount;
        maxGoldenCupAmountSlider.value = PlayerSettings.MaxGoldenCupAmount;

        SetVolumeHandleText(masterVolumeHandleText, PlayerSettings.MasterVolume);
        SetVolumeHandleText(musicVolumeHandleText, PlayerSettings.MusicVolume);
        SetVolumeHandleText(effectsVolumeHandleText, PlayerSettings.EffectsVolume);
        SetVolumeHandleText(UIVolumeHandleText, PlayerSettings.UIVolume);
        startHealthAmountHandleText.text = PlayerSettings.StartHealthAmount.ToString();
        maxGoldenCupAmountHandleText.text = PlayerSettings.MaxGoldenCupAmount.ToString();

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

    public void SetMuteOtherFields(bool isMuteOtherFields)
    {
        PlayerSettings.IsMuteOtherFields = isMuteOtherFields;
    }

    public void SetStartHealthAmount(float healthAmount)
    {
        PlayerSettings.StartHealthAmount = (int)healthAmount;
        startHealthAmountHandleText.text = healthAmount.ToString();

        if (currentPageIndex == 1)
            FileSaveSystem.DeleteSavedProgress();
    }

    public void SetMaxGoldenCupAmount(float goldenCupAmount)
    {
        PlayerSettings.MaxGoldenCupAmount = (int)goldenCupAmount;
        maxGoldenCupAmountHandleText.text = goldenCupAmount.ToString();

        if (currentPageIndex == 1)
            FileSaveSystem.DeleteSavedProgress();
    }

    private void SetVolumeHandleText(TMP_Text volumeHandleText, float volume)
    {
        volumeHandleText.text =
            Mathf.Lerp(MIN_VOLUME_SLIDER_VALUE, MAX_VOLUME_SLIDER_VALUE, volume / 100)
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
