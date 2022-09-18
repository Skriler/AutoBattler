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
        protected HealthBar healthBar;

        protected bool isAttacking = false;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            animator = GetComponent<Animator>();

            Id = Guid.NewGuid().ToString("N");

            SetÑharacteristics();

            healthBar = Instantiate(barPrefab, this.transform);
            healthBar.Setup(this.transform, characteristics.MaxHealth);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
                TakeDamage(17);

            if (Input.GetKeyDown(KeyCode.K))
                Attack();

            if (Input.GetKeyDown(KeyCode.F))
                Death();

            if (Input.GetKeyDown(KeyCode.R))
                Resurrect();
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

        public bool IsAlive() => Health > 0;

        public void TakeDamage(int damageAmount)
        {
            Health -= damageAmount;
            healthBar.UpdateBar(Health);
        }

        protected void FindTarget(BaseUnit[,] units)
        {

        }

        public void Attack()
        {
            if (isAttacking)
                return;

            animator.SetTrigger("attackTrigger");
            StartCoroutine(WaitCoroutine());
        }

        private IEnumerator WaitCoroutine()
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
    }
}
