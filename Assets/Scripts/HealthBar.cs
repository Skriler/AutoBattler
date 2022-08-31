using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AutoBattler.EventManagers;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform health;
    [SerializeField] private Vector3 offset;

    private Transform target;
    private float maxHealthAmount;

    private void OnEnable()
    {
        UnitsEventManager.OnDraggedUnitChangedPosition += ChangeBarPosition;
    }

    private void OnDestroy()
    {
        UnitsEventManager.OnDraggedUnitChangedPosition -= ChangeBarPosition;
    }

    public void Setup(Transform target, float maxHealthAmount)
    {
        this.maxHealthAmount = maxHealthAmount;
        this.target = target;
        transform.position = target.position + offset;
    }

    public void UpdateBar(float healthAmount)
    {
        healthAmount = (healthAmount > 0) ? healthAmount : 0;
        float scale = healthAmount / maxHealthAmount;
        Vector3 scaleVector = health.transform.localScale;
        scaleVector.x = scale;
        health.transform.localScale = scaleVector;
    }

    private void ChangeBarPosition(Vector3 position)
    {
        if (target == null)
            return;

        transform.position = target.position + offset;
    }
}
