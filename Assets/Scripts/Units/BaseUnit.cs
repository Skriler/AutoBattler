using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    [SerializeField] private HealthBar barPrefab;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float attackDamage = 10;
    [SerializeField] private float attackSpeed = 5;
    [SerializeField] private float attackTime;

    public string Id { get; protected set; }
    public int Cost { get; protected set; } = 1;
    public float Health { get; protected set; }

    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected HealthBar healthBar;

    private void Start()
    {
        Id = Guid.NewGuid().ToString("N");

        Health = maxHealth;

        healthBar = Instantiate(barPrefab, this.transform);
        healthBar.Setup(this.transform, maxHealth);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.N))
            TakeDamage(17);
    }

    public bool IsAlive() => Health > 0;

    public void TakeDamage(int damageAmount)
    {
        Health -= damageAmount;
        healthBar.UpdateBar(Health);
    }
}
