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

        private Camera currentCamera;

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
            currentCamera = Camera.current;
        }

        public void InstantiateTakenDamageText(BaseUnit unit, float healthAmount)
        {
            UITakenDamageText takenDamageText = Instantiate(takenDamagePrefab);

            takenDamageText.transform.SetParent(gameObject.transform, false);
            takenDamageText.Setup(healthAmount, unit.transform.position);
        }
    }
}
