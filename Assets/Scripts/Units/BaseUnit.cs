using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    //[SerializeField] private int baseDamage = 2;
    //[SerializeField] private int baseHealth = 10;

    public string Id { get; protected set; }

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Start()
    {
        Id = Guid.NewGuid().ToString("N");
    }
}
