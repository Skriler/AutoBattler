using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler.Units
{
    public class BaseUnit : MonoBehaviour
    {
        [SerializeField] private HealthBar barPrefab;
        [SerializeField] private float maxHealth = 100;
        [SerializeField] private float attackDamage = 10;
        [SerializeField] private float attackSpeed = 3f;
        [SerializeField] private float attackTime;

        public string Id { get; protected set; }
        public int Cost { get; protected set; } = 1;
        public float Health { get; protected set; }

        protected SpriteRenderer spriteRenderer;
        protected Animator animator;
        protected HealthBar healthBar;

        protected bool isAttacking = false;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            Id = Guid.NewGuid().ToString("N");

            Health = maxHealth;

            healthBar = Instantiate(barPrefab, this.transform);
            healthBar.Setup(this.transform, maxHealth);
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
            yield return new WaitForSeconds(attackSpeed);
            isAttacking = false;
        }

        public void Death()
        {
            animator.SetTrigger("deathTrigger");
        }

        public void Resurrect()
        {
            animator.SetTrigger("idleTrigger");
            Health = maxHealth;
        }
    }
}
