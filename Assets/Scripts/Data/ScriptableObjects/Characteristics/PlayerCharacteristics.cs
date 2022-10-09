using UnityEngine;

namespace AutoBattler.Data.ScriptableObjects.Characteristics
{
    [CreateAssetMenu(
        fileName = "Player Characteristics",
        menuName = "Custom/Characteristics/PlayerCharacteristics"
        )]
    public class PlayerCharacteristics : ScriptableObject
    {
        [SerializeField] private int startHealth = 10;
        [SerializeField] private int startGold = 0;
        [SerializeField] private int startTavernTier = 1;
        [SerializeField] private int maxHealth = 99;
        [SerializeField] private int maxGold = 99;
        [SerializeField] private int maxTavernTier = 5;

        public int StartHealth => startHealth;
        public int StartGold => startGold;
        public int StartTavernTier => startTavernTier;
        public int MaxHealth => maxHealth;
        public int MaxGold => maxGold;
        public int MaxTavernTier => maxTavernTier;
    }
}
