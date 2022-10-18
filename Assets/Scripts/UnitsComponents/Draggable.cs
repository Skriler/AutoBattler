using UnityEngine;
using AutoBattler.Data.Units;
using AutoBattler.EventManagers;
using AutoBattler.UI.Tooltips;

namespace AutoBattler.UnitsComponents
{
    public class Draggable : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private int dragSortingOrder = 10;
        [SerializeField] private Vector3 dragOffset = new Vector3(-0.1f, -0.1f, 0);

        private SpriteRenderer spriteRenderer;
        private BaseUnit unit;
        private Camera mainCamera;

        private Vector3 startPosition;
        private int startSortingOrder;

        public bool IsActive { get; set; } = true;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            unit = GetComponent<BaseUnit>();
            mainCamera = Camera.main;
        }

        public void OnStartDrag()
        {
            if (!IsActive)
                return;

            startPosition = transform.position;
            startSortingOrder = spriteRenderer.sortingOrder;
            spriteRenderer.sortingOrder = dragSortingOrder;

            UIUnitTooltip.Instance.Hide();
        }

        public void OnDragging()
        {
            if (!IsActive)
                return;

            Vector3 currentPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            currentPosition.z = 0;

            UnitsEventManager.SendDraggedUnitChangedPosition(currentPosition);

            currentPosition += dragOffset;
            transform.position = currentPosition;

            UIUnitTooltip.Instance.Hide();
        }

        public void OnEndDrag()
        {
            if (!IsActive)
                return;

            Vector3 currentPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            transform.position = startPosition;
            spriteRenderer.sortingOrder = startSortingOrder;

            UnitsEventManager.SendUnitEndDrag(unit, currentPosition);
        }
    }
}
