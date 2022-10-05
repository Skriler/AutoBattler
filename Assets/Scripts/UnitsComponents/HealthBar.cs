using UnityEngine;
using AutoBattler.EventManagers;
using AutoBattler.UI;

namespace AutoBattler.UnitsComponents
{
    public class HealthBar : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Transform health;
        [SerializeField] private Transform lostHealth;
        [SerializeField] private Transform stamina;

        [Header("Parameters")]
        [SerializeField] private Vector3 offset;

        private Transform target;
        private float maxHealthAmount;
        private float maxStaminaAmount;
        private float previousHealthAmount;

        private void Awake()
        {
            UnitsEventManager.OnDraggedUnitChangedPosition += ChangeBarPosition;
        }

        private void OnDestroy()
        {
            UnitsEventManager.OnDraggedUnitChangedPosition -= ChangeBarPosition;
        }

        public void Hide() => gameObject.SetActive(false);
        public void Show() => gameObject.SetActive(true);

        public void Setup(Transform target, float maxHealthAmount, float maxStaminaAmount)
        {
            this.target = target;
            transform.position = target.position + offset;

            this.maxHealthAmount = maxHealthAmount;
            previousHealthAmount = maxHealthAmount;
            this.maxStaminaAmount = maxStaminaAmount;

            UpdateHealth(maxHealthAmount);
            UpdateStamina(0);
        }

        public void UpdateHealth(float healthAmount)
        {
            if (target == null)
                return;

            healthAmount = (healthAmount > 0) ? healthAmount : 0;
            UpdateLostHealth(healthAmount);

            float scale = healthAmount / maxHealthAmount;
            Vector3 scaleVector = health.transform.localScale;
            scaleVector.x = scale;
            health.transform.localScale = scaleVector;
        }

        public void UpdateStamina(float staminaAmount)
        {
            if (target == null)
                return;

            staminaAmount = (staminaAmount > maxStaminaAmount) ? maxStaminaAmount : staminaAmount;

            float scale = staminaAmount / maxStaminaAmount;
            Vector3 scaleVector = stamina.transform.localScale;
            scaleVector.x = scale;
            stamina.transform.localScale = scaleVector;
        }

        private void UpdateLostHealth(float healthAmount)
        {
            if (target == null)
                return;

            float scale = previousHealthAmount / maxHealthAmount;
            Vector3 scaleVector = lostHealth.transform.localScale;

            scaleVector.x = scale;
            lostHealth.transform.localScale = scaleVector;

            previousHealthAmount = healthAmount;
        }

        private void ChangeBarPosition(Vector3 position)
        {
            if (target == null)
                return;

            transform.position = target.position + offset;
        }
    }
}
