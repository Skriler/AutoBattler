using System.Collections;
using UnityEngine;
using TMPro;
using AutoBattler.Managers;

namespace AutoBattler.UI.ResultNotifications
{
    public abstract class RoundResultNotification : Manager<RoundResultNotification>
    {
        [Header("Parameters")]
        [SerializeField] protected float showNotificationTime = 3.5f;
        [SerializeField] protected float hideNotificationTime = 1.5f;
        [SerializeField] protected float hideNotificationInterval = 0.05f;

        protected CanvasGroup canvasGroup;

        protected WaitForSeconds hideNotificationTick;
        protected float hidingNotificationTime;

        protected override void Awake()
        {
            base.Awake();

            canvasGroup = GetComponent<CanvasGroup>();
            hideNotificationTick = new WaitForSeconds(hideNotificationInterval);

            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            StartCoroutine(HideNotificationCoroutine());
        }

        protected IEnumerator HideNotificationCoroutine()
        {
            yield return new WaitForSeconds(showNotificationTime);

            hidingNotificationTime = 0;

            while (hidingNotificationTime < hideNotificationTime)
            {
                hidingNotificationTime += hideNotificationInterval;
                canvasGroup.alpha = 1 - hidingNotificationTime / hideNotificationTime;
                yield return hideNotificationTick;
            }
        }

        protected string GetCharacteristicStr(int characteristicAmount)
        {
            return characteristicAmount > 0 ?
                "+" + characteristicAmount.ToString() :
                characteristicAmount.ToString();
        }
    }
}
