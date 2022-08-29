using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform health;
    [SerializeField] private Vector3 offset;

    private Transform target;
    private float maxHealthAmount;

    public void Setup(Transform target, float maxHealthAmount)
    {
        this.maxHealthAmount = maxHealthAmount;
        this.target = target;
    }

    public void UpdateBar(float healthAmount)
    {
        float scale = healthAmount / maxHealthAmount;
        Vector3 scaleVector = health.transform.localScale;
        scaleVector.x = scale;
        health.transform.localScale = scaleVector;
    }

    private void Update()
    {
        if (target == null)
            return;

        transform.position = target.position + offset;
    }
}
