using UnityEngine;

namespace AutoBattler.Data.ScriptableObjects.Characteristics
{
    [CreateAssetMenu(fileName = "Player Characteristics", menuName = "Custom/PlayerCharacteristics")]
    public class PlayerCharacteristics : ScriptableObject
    {
        [SerializeField] private int startHealth = 50;
        [SerializeField] private int startGold = 0;
        [SerializeField] private int startTavernTier = 1;
        [SerializeField] private int maxTavernTier = 5;
        [SerializeField] private int levelUpTavernTierCost = 5;

        public int StartHealth => startHealth;
        public int StartGold => startGold;
        public int StartTavernTier => startTavernTier;
        public int MaxTavernTier => maxTavernTier;
        public int LevelUpTavernTierCost => levelUpTavernTierCost;
    }
}
