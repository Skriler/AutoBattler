using System.Collections.Generic;
using UnityEngine;
using AutoBattler.Data.Enums;
using AutoBattler.Data.ScriptableObjects.Structs;

namespace AutoBattler.Data.ScriptableObjects.Characteristics
{
    [CreateAssetMenu(
        fileName = "Unit Characteristics",
        menuName = "Custom/Characteristics/UnitCharacteristics"
        )]
    public class UnitCharacteristics : ScriptableObject
    {
        [Header("Shop Characteristics")]
        [SerializeField] private string title;
        [SerializeField] [TextArea(5, 10)] private string attackDescription;
        [SerializeField] private int cost;
        [SerializeField] private int tavernTier;

        [Header("Unit Diversity")]
        [SerializeField] private UnitRace race;
        [SerializeField] private UnitSpecification specification;

        [Header("Fight Characteristics")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private DamageType damageType;
        [SerializeField] private float attackDamage = 10f;
        [SerializeField] private float attackSpeed = 5f;
        [SerializeField] private DamageTypeProtection[] damageTypesProtectionPercentage;

        private string description;

        public string Title => title;
        public string Description => GetDescription();
        public int Cost => cost;
        public int TavernTier => tavernTier;

        public UnitRace Race => race;
        public UnitSpecification Specification => specification;

        public float MaxHealth => maxHealth;
        public DamageType DamageType => damageType;
        public float AttackDamage => attackDamage;
        public float AttackSpeed => attackSpeed;
        public DamageTypeProtection[] DamageTypesProtectionPercentage => damageTypesProtectionPercentage;

        private string GetDescription()
        {
            if (description == string.Empty)
            {
                description = Race.ToString() + ". " + Specification.ToString() + ".\n";
                description += attackDescription;
            }

            return description;
        }
    }
}
