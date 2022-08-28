using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AutoBattler.UI
{
    public class UITrashCan : MonoBehaviour
    {
        private EventSystem eventSystem;

        private void OnEnable()
        {
            UnitsEventManager.OnUnitEndDrag += SellUnit;
        }

        private void OnDestroy()
        {
            UnitsEventManager.OnUnitEndDrag -= SellUnit;
        }

        private void Start()
        {
            eventSystem = EventSystem.current;
        }

        private void SellUnit(BaseUnit unit, Vector3 position)
        {
            //if (!EventSystem.current.IsPointerOverGameObject())
            //    return;

            //if (EventSystem.current.currentSelectedGameObject.CompareTag(gameObject.tag))
            //    return;

            //if (eventSystem.IsPointerOverGameObject() &&
            //    eventSystem.currentSelectedGameObject != null &&
            //    eventSystem.currentSelectedGameObject.CompareTag(tag))
            //{
            //    Debug.Log("sell unit");
            //}
        }
    }
}
