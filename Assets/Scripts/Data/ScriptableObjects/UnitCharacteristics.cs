using UnityEngine;

namespace AutoBattler.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Unit Characteristics", menuName = "Custom/UnitCharacteristics")]
    public class UnitCharacteristics : ScriptableObject
    {
        [Header("Shop Stats")]
        [SerializeField] private string title;
        [SerializeField] private int cost;
        [SerializeField] private int tavernTier;

        [Header("Fight Stats")]
        [SerializeField] private float maxHealth = 100;
        [SerializeField] private float attackDamage = 10;
        [SerializeField] private float attackSpeed = 3f;

        public string Title => title;
        public int Cost => cost;
        public int TavernTier => tavernTier;

        public float MaxHealth => maxHealth;
        public float AttackDamage => attackDamage;
        public float AttackSpeed => attackSpeed;
    }
}
