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
        [SerializeField] private float maxDistance = 0.3f;

        private Vector3 startPosition;
        private Vector3 targetPosition;
        private float time;

        private void Start()
        {
            //Vector3 screenPosition = Camera.current.WorldToScreenPoint(transform.position);

            //float x = screenPosition.x - (Screen.width / 2);
            //float y = screenPosition.y - (Screen.height / 2);
            //float scaleFactor = canvas.scaleFactor;
            //textDamage.rectTransform.anchoredPosition = new Vector2(x, y) / scaleFactor;

            startPosition = transform.position;

            float distance = Random.Range(minDistance, maxDistance);
            targetPosition = startPosition + new Vector3(distance, distance);

            StartCoroutine(RemoveTextCoroutine());
        }

        //private void Update()
        //{
        //    time = Mathf.Sin(Time.deltaTime / lifetime);

        //    transform.position = Vector3.Lerp(startPosition, targetPosition, time);
        //    transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time);
        //}

        public void Setup(float damage)
        {
            textDamage.text = damage.ToString();
            textDamage.color = startColor;
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
