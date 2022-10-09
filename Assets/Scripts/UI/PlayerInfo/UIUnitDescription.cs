using UnityEngine;
using TMPro;
using AutoBattler.Managers;

public class UIUnitDescription : Manager<UIUnitDescription>
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI textDescription;

    private void Start()
    {
        textDescription.text = string.Empty;
    }

    public void Show(string description) => textDescription.text = description;

    public void Hide() => textDescription.text = string.Empty;
}
