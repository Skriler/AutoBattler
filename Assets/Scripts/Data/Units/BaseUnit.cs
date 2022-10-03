using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AutoBattler.Data.ScriptableObjects.Characteristics;
using AutoBattler.Data.ScriptableObjects.Structs;
using AutoBattler.UnitsComponents;
using AutoBattler.Data.Enums;
using AutoBattler.UI.Tooltips;
using AutoBattler.EventManagers;

namespace AutoBattler.Data.Units
{
    public abstract class BaseUnit : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private HealthBar barPrefab;
        [SerializeField] protected UnitCharacteristics characteristics;

        [Header("Parameters")]
        [SerializeField] protected float staminaRegenInterval = 0.05f;
        [SerializeField] protected float dealDamageInterval = 0.4f;

        public string Id { get; protected set; }
        public string Title { get; protected set; }
        public int Cost { get; protected set; }
        public UnitRace Race { get; protected set; }
        public UnitSpecification Specification { get; protected set; }
        public float MaxHealth { get; protected set; }
        public float Health { get; protected set; }
        public DamageType DamageType { get; protected set; }
        public float AttackDamage { get; protected set; }
        public float AttackSpeed { get; protected set; }
        public float Stamina { get; protected set; }

        protected Dictionary<DamageType, int> damageTypesProtectionPercentage;

        protected SpriteRenderer spriteRenderer;
        protected Animator animator;
        protected Draggable draggable;

        protected HealthBar healthBar;
        protected WaitForSeconds staminaRegenTick;
        protected Coroutine regenStamina;

        protected bool isFightMode = false;
        protected BaseUnit[,] enemyUnits;

        protected abstract void FindTarget(BaseUnit[,] enemyUnits);
        protected abstract bool HasTargetedEnemy();
        protected abstract void DealDamageToTargetedEnemy();
        protected abstract void CheckTargetedEnemy();

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            draggable = GetComponent<Draggable>();
            staminaRegenTick = new WaitForSeconds(staminaRegenInterval);

            Id = Guid.NewGuid().ToString("N");
            Set—haracteristics();

            healthBar = Instantiate(barPrefab, this.transform);
            healthBar.Setup(this.transform, characteristics.MaxHealth, characteristics.AttackSpeed);
            healthBar.Hide();
        }

        private void Update()
        {
            if (!isFightMode || !IsAlive())
                return;

            CheckTargetedEnemy();

            if (!HasTargetedEnemy())
            {
                FindTarget(enemyUnits);
            }

            if (!HasEnoughStamina() && regenStamina == null)
                regenStamina = StartCoroutine(RegenStaminaCoroutine());

            if (HasEnoughStamina() && HasTargetedEnemy())
            {
                Attack();
            }
        }

        public void MouseExit() => UIUnitTooltip.Instance.Hide();

        public void MouseEnter()
        {
            UIUnitTooltip.Instance.Show();
            UIUnitTooltip.Instance.Setup(this);
        }

        private void Set—haracteristics()
        {
            Title = characteristics.Title;
            Cost = characteristics.Cost;

            Race = characteristics.Race;
            Specification = characteristics.Specification;

            MaxHealth = characteristics.MaxHealth;
            Health = characteristics.MaxHealth;
            DamageType = characteristics.DamageType;
            AttackDamage = characteristics.AttackDamage;
            AttackSpeed = characteristics.AttackSpeed;
            Stamina = 0;

            damageTypesProtectionPercentage = new Dictionary<DamageType, int>();
            DamageTypeProtection[] damageTypeProtection = characteristics.DamageTypesProtectionPercentage;

            for (int i = 0; i < damageTypeProtection.Length; ++i)
            {
                DamageTypeProtection currentDamageTypeProtection = damageTypeProtection[i];
                damageTypesProtectionPercentage.Add(currentDamageTypeProtection.damageType, currentDamageTypeProtection.protectionPercentage);
            }
        }

        public float GetDamageTypeProtection(DamageType damageType) => damageTypesProtectionPercentage[damageType];

        public bool HasEnoughStamina() => Stamina >= AttackSpeed;

        public void HideHealthBar() => healthBar.Hide();

        public void ShowHealthBar() => healthBar.Show();

        public bool IsAlive() => Health > 0;

        public void TakeDamage(float damage, DamageType damageType)
        {
            if (Health == 0)
                return;

            CalculateTakenDamage(ref damage, damageType);
            damage = (float)Math.Round(damage, 1);

            Health -= damage;
            Health = Health < 0 ? 0 : (float)Math.Round(Health, 1);

            healthBar.UpdateHealth(Health);

            UnitsEventManager.OnUnitTookDamage(this, damage, damageType);

            if (UIUnitTooltip.Instance.CurrentUnit == this)
                UIUnitTooltip.Instance.Setup(this);

            if (!IsAlive())
                Death();
        }

        private void CalculateTakenDamage(ref float damage, DamageType damageType)
        {
            float damageProtection—oefficient = damageType switch
            {
                DamageType.Fire => GetDamageTypeProtection(DamageType.Fire),
                DamageType.Ice => GetDamageTypeProtection(DamageType.Ice),
                DamageType.Chaos => GetDamageTypeProtection(DamageType.Chaos),
                DamageType.Purify => GetDamageTypeProtection(DamageType.Purify),
                _ => 0,
            };

            damageProtection—oefficient /= 100;
            float takenDamage—oefficient = 1 - damageProtection—oefficient;

            damage *= takenDamage—oefficient;
        }

        public void ApplyCharacteristicBonus(UnitCharacteristic characteristic, float addedPointsAmount)
        {
            switch (characteristic)
            {
                case (UnitCharacteristic.AttackDamage):
                    AttackDamage += addedPointsAmount;
                    AttackDamage = (float)Math.Round(AttackDamage, 1);
                    break;
                case (UnitCharacteristic.AttackSpeed):
                    AttackSpeed -= addedPointsAmount;
                    AttackSpeed = (float)Math.Round(AttackSpeed, 1);
                    break;
                case (UnitCharacteristic.Health):
                    MaxHealth += addedPointsAmount;
                    MaxHealth = (float)Math.Round(MaxHealth, 1);
                    Health = MaxHealth;
                    break;
            }
            
            healthBar.Setup(this.transform, MaxHealth, AttackSpeed);
        }

        public void Attack()
        {
            if (!HasEnoughStamina() || !HasTargetedEnemy())
                return;

            Stamina = 0;
            animator.SetTrigger("attackTrigger");
            StartCoroutine(DealDamageCoroutine());

            if (regenStamina != null)
                StopCoroutine(regenStamina);

            regenStamina = StartCoroutine(RegenStaminaCoroutine());
        }

        private IEnumerator DealDamageCoroutine()
        {
            yield return new WaitForSeconds(dealDamageInterval);
            DealDamageToTargetedEnemy();
        }

        private IEnumerator RegenStaminaCoroutine()
        {
            while (Stamina < AttackSpeed)
            {
                Stamina += staminaRegenInterval;
                healthBar.UpdateStamina(Stamina);
                yield return staminaRegenTick;
            }

            regenStamina = null;
        }

        public void Death()
        {
            animator.SetTrigger("deathTrigger");
            healthBar.Hide();
        }

        public void Resurrect()
        {
            Health = MaxHealth;
            Stamina = 0;

            animator.SetTrigger("idleTrigger");

            healthBar.Show();
            healthBar.UpdateHealth(Health);
            healthBar.UpdateStamina(Stamina);
        }

        public void FlipOnX()
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        public void EnterFightMode(BaseUnit[,] enemyUnits)
        {
            draggable.IsActive = false;
            isFightMode = true;

            this.enemyUnits = enemyUnits;
        }

        public void ExitFightMode()
        {
            draggable.IsActive = true;
            isFightMode = false;

            Resurrect();
        }

        public void Click() { }
    }
}
