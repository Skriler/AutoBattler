using UnityEngine;
using AutoBattler.EventManagers;
using AutoBattler.Data.Units;

namespace AutoBattler.UI.Effects
{
    public class UIEffectsContainer : MonoBehaviour
    {
        [SerializeField] private UITakenDamageText takenDamagePrefab;

        private void OnEnable()
        {
            UnitsEventManager.OnUnitTookDamage += InstantiateTakenDamageText;
        }

        private void OnDestroy()
        {
            UnitsEventManager.OnUnitTookDamage -= InstantiateTakenDamageText;
        }

        public void InstantiateTakenDamageText(BaseUnit unit, float healthAmount)
        {
            UITakenDamageText takenDamageText = Instantiate(takenDamagePrefab);
            takenDamageText.transform.SetParent(gameObject.transform, false);

            Vector3 canvasPosition = RectTransformUtility.WorldToScreenPoint(Camera.current, unit.transform.position);
            takenDamageText.Setup(healthAmount, canvasPosition);
        }
    }
}
