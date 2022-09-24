using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AutoBattler.EventManagers;
using AutoBattler.Data.Units;

namespace AutoBattler.UI
{
    public class UIEffectsContainer : MonoBehaviour
    {
        [SerializeField] private UITakenDamageText takenDamagePrefab;

        private Camera mainCamera;

        private void OnEnable()
        {
            UnitsEventManager.OnUnitTookDamage += InstantiateTakenDamageText;
        }

        private void OnDestroy()
        {
            UnitsEventManager.OnUnitTookDamage -= InstantiateTakenDamageText;
        }

        private void Start()
        {
            mainCamera = Camera.main;
        }

        public void InstantiateTakenDamageText(BaseUnit unit, float healthAmount)
        {
            UITakenDamageText takenDamageText = Instantiate(takenDamagePrefab);
            takenDamageText.Setup(healthAmount);

            takenDamageText.transform.position = mainCamera.WorldToScreenPoint(unit.transform.position);
            takenDamageText.transform.SetParent(gameObject.transform, false);
        }
    }
}
