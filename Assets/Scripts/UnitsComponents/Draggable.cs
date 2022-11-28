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
        public bool IsDragging { get; private set; } = false;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            unit = GetComponent<BaseUnit>();
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (!IsActive)
                return;

            if (Input.GetMouseButtonUp(0) && IsDragging)
            {
                IsDragging = false;
                OnEndDrag();
                return;
            }

            CheckMouseInput();
        }

        private void CheckMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                BaseUnit raycastUnit = Physics2D.Raycast(mousePosition, Vector2.one)
                    .collider
                    .GetComponent<BaseUnit>();

                if (raycastUnit != null && raycastUnit.Id == unit.Id)
                {
                    IsDragging = false;
                    return;
                }

                IsDragging = true;
                OnStartDrag();
            }

            if (Input.GetMouseButton(0) && IsDragging)
                OnDragging();
        }

        private void OnStartDrag()
        {
            if (!IsActive)
                return;

            startPosition = transform.position;
            startSortingOrder = spriteRenderer.sortingOrder;
            spriteRenderer.sortingOrder = dragSortingOrder;

            UIUnitTooltip.Instance.Hide();
        }

        private void OnDragging()
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

        private void OnEndDrag()
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
