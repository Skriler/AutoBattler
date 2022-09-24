using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AutoBattler.UI
{
    public class UITakenDamageText : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private TextMeshProUGUI textDamage;
        [SerializeField] private Color startColor;
        [SerializeField] private Color fadedColor;

        [Header("Parameters")]
        [SerializeField] private float lifetime = 150f;
        [SerializeField] private float minDistance = 0.1f;
        [SerializeField] private float maxDistance = 0.7f;

        private Vector3 startPosition;
        private Vector3 targetPosition;
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

        public void Setup(float damage, Vector3 position)
        {
            textDamage.text = damage.ToString();
            textDamage.color = startColor;
            transform.position = RectTransformUtility.WorldToScreenPoint(
                Camera.current,
                position);
        }

        private IEnumerator RemoveTextCoroutine()
        {
            yield return new WaitForSeconds(lifetime / 2);
            textDamage.color = fadedColor;

            yield return new WaitForSeconds(lifetime / 2);
            Destroy(gameObject);
        }
    }
}
