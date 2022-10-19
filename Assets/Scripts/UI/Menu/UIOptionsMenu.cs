using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIOptionsMenu : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;

    private void Start()
    {
        SetResolutionDropdownItems();
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
}
