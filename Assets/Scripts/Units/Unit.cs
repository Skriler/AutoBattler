using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Animator animator;

    public int damage = 2;
    public int health = 10;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //animator.SetBool("isAttacking", true);
        }
        else
        {
            //animator.SetBool("isAttacking", false);
        }
    }
}
