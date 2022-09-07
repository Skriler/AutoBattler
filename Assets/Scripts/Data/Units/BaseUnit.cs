using System;
using System.Collections;
using UnityEngine;
using AutoBattler.Data.ScriptableObjects;
using AutoBattler.UnitsComponents;

namespace AutoBattler.Data.Units
{
    public class BaseUnit : MonoBehaviour
    {
        [SerializeField] private HealthBar barPrefab;
        [SerializeField] private UnitCharacteristics characteristics;

        public string Id { get; protected set; }
        public int Cost { get; protected set; } = 1;
        public float Health { get; protected set; }

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

            Health = characteristics.MaxHealth;

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
            Health = characteristics.MaxHealth;
            healthBar.Show();
            healthBar.UpdateBar(Health);
        }

        public void FlipOnX()
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
}
