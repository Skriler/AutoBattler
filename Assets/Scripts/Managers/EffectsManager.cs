using UnityEngine;
using AutoBattler.EventManagers;
using AutoBattler.Data.Units;
using AutoBattler.Data.Enums;
using AutoBattler.UnitsComponents;

namespace AutoBattler.Managers
{
    public class EffectsManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private TakenDamageContainer takenDamageContainerPrefab;

        private void Awake()
        {
            UnitsEventManager.OnUnitTookDamage += InstantiateTakenDamageText;
        }

        private void OnDestroy()
        {
            UnitsEventManager.OnUnitTookDamage -= InstantiateTakenDamageText;
        }

        public void InstantiateTakenDamageText(BaseUnit unit, float healthAmount, DamageType damageType)
        {
            if (unit == null)
                return;

            TakenDamageContainer takenDamageContainer = Instantiate(takenDamageContainerPrefab, unit.gameObject.transform);
            takenDamageContainer.Setup(healthAmount, damageType);
        }
    }
}
