using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using AutoBattler.Data.Units;
using AutoBattler.EventManagers;

namespace AutoBattler.UI.PlayerInfo
{
    public class UITrashCan : MonoBehaviour
    {
        [SerializeField] private Image openedTrashCan;

        private void OnEnable()
        {
            UnitsEventManager.OnDraggedUnitChangedPosition += ChangeImage;
            UnitsEventManager.OnUnitEndDrag += SellUnit;
        }

        private void OnDestroy()
        {
            UnitsEventManager.OnDraggedUnitChangedPosition += ChangeImage;
            UnitsEventManager.OnUnitEndDrag -= SellUnit;
        }

        private void ChangeImage(Vector3 positon)
        {
            
        }

        private void SellUnit(BaseUnit unit, Vector3 positon)
        {
            
            //Destroy(unit.gameObject);
            //UnitsEventManager.OnUnitSold(unit);
        }

        //private void SellUnit(BaseUnit unit, Vector3 position)
        //{
        //    if (!EventSystem.current.IsPointerOverGameObject())
        //        return;

        //    if (EventSystem.current.currentSelectedGameObject.CompareTag(gameObject.tag))
        //        return;

        //    if (eventSystem.IsPointerOverGameObject() &&
        //        eventSystem.currentSelectedGameObject != null &&
        //        eventSystem.currentSelectedGameObject.CompareTag(tag))
        //    {
        //        Debug.Log("sell unit");
        //    }
        //}
    }
}
