using System.Collections;
using TMPro;
using UnityEngine;
using AutoBattler.UI.Tooltips;
using AutoBattler.Data.Enums;

namespace AutoBattler.UI.Effects
{
    public class UITakenDamageText : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private TextMeshProUGUI textDamage;

        [Header("Parameters")]
        [SerializeField] private float lifetime = 2.5f;
        [SerializeField] private float minDistance = -200f;
        [SerializeField] private float maxDistance = 200f;

        [Header("Fire Colors")]
        [SerializeField] private Color startFireColor;
        [SerializeField] private Color fadedFireColor;

        [Header("Ice Colors")]
        [SerializeField] private Color startIceColor;
        [SerializeField] private Color fadedIceColor;

        [Header("Chao Colors")]
        [SerializeField] private Color startChaosColor;
        [SerializeField] private Color fadedChaosColor;

        [Header("Purify Colors")]
        [SerializeField] private Color startPurifyColor;
        [SerializeField] private Color fadedPurifyColor;

        private Vector3 startPosition;
        private Vector3 targetPosition;
        private DamageType currentDamageType;
        private float timer = 0;

        private void Start()
        {
            startPosition = transform.position;

            float distance = Random.Range(minDistance, maxDistance);
            targetPosition = startPosition + new Vector3(distance, distance);

            StartCoroutine(RemoveTextCoroutine());
        }

        private void Update()
        {
            timer += Time.deltaTime;

            transform.position = Vector3.Lerp(startPosition, targetPosition, timer / lifetime);
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timer / lifetime);
        }

        public void MouseExit() => UIUnitTooltip.Instance.Hide();

        public void MouseEnter() => UIUnitTooltip.Instance.Show();

        public void Setup(float damage, Vector3 position, DamageType damageType)
        {
            textDamage.text = damage.ToString();
            transform.position = position;

            textDamage.color = damageType switch
            {
                DamageType.Fire => startFireColor,
                DamageType.Ice => startIceColor,
                DamageType.Chaos => startChaosColor,
                DamageType.Purify => startPurifyColor,
                _ => new Color(),
            };
            currentDamageType = damageType;
        }

        private IEnumerator RemoveTextCoroutine()
        {
            yield return new WaitForSeconds(lifetime / 2);

            textDamage.color = currentDamageType switch
            {
                DamageType.Fire => fadedFireColor,
                DamageType.Ice => fadedIceColor,
                DamageType.Chaos => fadedChaosColor,
                DamageType.Purify => fadedPurifyColor,
                _ => new Color(),
            };

            yield return new WaitForSeconds(lifetime / 2);
            Destroy(gameObject);
        }
    }
}
