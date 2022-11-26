using System.Text;
using UnityEngine;
using TMPro;
using AutoBattler.Managers;
using AutoBattler.Data.ScriptableObjects.Characteristics;

public class UIUnitDescription : Manager<UIUnitDescription>
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI textDescription;

    StringBuilder descriptionBuilder;
    private UnitCharacteristics currentUnitCharacteristics;
    private string description;

    private void Start()
    {
        descriptionBuilder = new StringBuilder();
        textDescription.text = string.Empty;
    }

    public void Show(UnitCharacteristics unitCharacteristics)
    {
        if (currentUnitCharacteristics != unitCharacteristics)
        {
            descriptionBuilder.Clear();
            descriptionBuilder
                .Append(unitCharacteristics.Race.ToString())
                .Append(". ")
                .Append(unitCharacteristics.Specification.ToString())
                .AppendLine(".")
                .AppendLine(unitCharacteristics.AttackDescription);

            description = descriptionBuilder.ToString();
        }

        textDescription.text = description;
        currentUnitCharacteristics = unitCharacteristics;
    }

    public void Hide() => textDescription.text = string.Empty;
}
