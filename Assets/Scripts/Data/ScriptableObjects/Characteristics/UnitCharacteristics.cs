using UnityEngine;
using AutoBattler.Data.Enums;

namespace AutoBattler.Data.ScriptableObjects.Characteristics
{
    [CreateAssetMenu(fileName = "Unit Characteristics", menuName = "Custom/UnitCharacteristics")]
    public class UnitCharacteristics : ScriptableObject
    {
        [Header("Shop Characteristics")]
        [SerializeField] private string title;
        [SerializeField] private int cost;
        [SerializeField] private int tavernTier;

        [Header("Unit Diversity")]
        [SerializeField] private UnitRace race;
        [SerializeField] private UnitSpecification specification;

        [Header("Fight Characteristics")]
        [SerializeField] private float maxHealth = 100;
        [SerializeField] private float attackDamage = 10;
        [SerializeField] private float attackSpeed = 3f;

        public string Title => title;
        public int Cost => cost;
        public int TavernTier => tavernTier;

        public UnitRace Race => race;
        public UnitSpecification Specification => specification;

        public float MaxHealth => maxHealth;
        public float AttackDamage => attackDamage;
        public float AttackSpeed => attackSpeed;
    }
}
