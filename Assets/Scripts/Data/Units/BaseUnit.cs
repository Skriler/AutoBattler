using System;
using System.Collections;
using UnityEngine;
using AutoBattler.Data.ScriptableObjects.Characteristics;
using AutoBattler.UnitsComponents;
using AutoBattler.Data.Enums;
using AutoBattler.UI.Tooltips;

namespace AutoBattler.Data.Units
{
    public abstract class BaseUnit : MonoBehaviour
    {
        [SerializeField] private HealthBar barPrefab;
        [SerializeField] protected UnitCharacteristics characteristics;

        public string Id { get; protected set; }
        public string Title { get; protected set; }
        public int Cost { get; protected set; }
        public float MaxHealth { get; protected set; }
        public float Health { get; protected set; }
        public float AttackDamage { get; protected set; }
        public float AttackSpeed { get; protected set; }
        public UnitRace Race { get; protected set; }
        public UnitSpecification Specification { get; protected set; }

        protected SpriteRenderer spriteRenderer;
        protected Animator animator;
        protected Draggable draggable;
        protected HealthBar healthBar;

        protected bool isFightMode = false;
        protected bool isAttacking = false;
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

            Id = Guid.NewGuid().ToString("N");
            SetÑharacteristics();

            healthBar = Instantiate(barPrefab, this.transform);
            healthBar.Setup(this.transform, characteristics.MaxHealth);
            healthBar.Hide();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
                TakeDamage(17);

            if (Input.GetKeyDown(KeyCode.K))
                Attack();

            if (Input.GetKeyDown(KeyCode.R))
                Resurrect();

            if (!isFightMode)
                return;

            CheckTargetedEnemy();

            if (!HasTargetedEnemy())
            {
                FindTarget(enemyUnits);
            }

            if (!isAttacking && HasTargetedEnemy())
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

        private void SetÑharacteristics()
        {
            Title = characteristics.Title;
            Cost = characteristics.Cost;
            MaxHealth = characteristics.MaxHealth;
            Health = characteristics.MaxHealth;
            AttackDamage = characteristics.AttackDamage;
            AttackSpeed = characteristics.AttackSpeed;

            Race = characteristics.Race;
            Specification = characteristics.Specification;
        }

        public void HideHealthBar() => healthBar.Hide();
        public void ShowHealthBar() => healthBar.Show();

        public bool IsAlive() => Health > 0;

        public void TakeDamage(float damageAmount)
        {
            if (Health == 0)
                return;

            Health -= damageAmount;
            Health = Health < 0 ? 0 : Health;

            healthBar.UpdateBar(Health);

            if (!IsAlive())
                Death();
        }

        public void Attack()
        {
            if (isAttacking || !HasTargetedEnemy())
                return;

            DealDamageToTargetedEnemy();
            animator.SetTrigger("attackTrigger");
            StartCoroutine(AttackCoroutine());
        }

        private IEnumerator AttackCoroutine()
        {
            isAttacking = true;
            yield return new WaitForSeconds(characteristics.AttackSpeed);
            isAttacking = false;
        }

        public void Death()
        {
            animator.SetTrigger("deathTrigger");
            healthBar.Hide();
        }

        public void Resurrect()
        {
            animator.SetTrigger("idleTrigger");
            Health = MaxHealth;
            healthBar.Show();
            healthBar.UpdateBar(Health);
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
    }
}
