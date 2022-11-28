using UnityEngine;

namespace AutoBattler.Data.ScriptableObjects.Characteristics
{
    [CreateAssetMenu(
        fileName = "Member Characteristics",
        menuName = "Custom/Characteristics/MemberCharacteristics"
        )]
    public class MemberCharacteristics : ScriptableObject
    {
        [SerializeField] private int startHealth = 10;
        [SerializeField] private int startGold = 3;
        [SerializeField] private int startTavernTier = 1;
        [SerializeField] private int maxHealth = 10;
        [SerializeField] private int maxGold = 99;
        [SerializeField] private int maxTavernTier = 5;
        [SerializeField] private int maxGoldenCupAmount = 10;

        public int StartHealth => startHealth;
        public int StartGold => startGold;
        public int StartTavernTier => startTavernTier;
        public int MaxHealth => maxHealth;
        public int MaxGold => maxGold;
        public int MaxTavernTier => maxTavernTier;
        public int MaxGoldenCupAmount => maxGoldenCupAmount;
    }
}
