using UnityEngine;

namespace AutoBattler.Data.ScriptableObjects.Characteristics
{
    [CreateAssetMenu(
        fileName = "UI Base Object Characteristics",
        menuName = "Custom/Characteristics/UIBaseObjectCharacteristics"
        )]
    public class UIBaseObjectCharacteristics : ScriptableObject
    {
        [SerializeField] private string title;
        [SerializeField] private string description;

        public string Title => title;
        public string Description => description;
    }
}
