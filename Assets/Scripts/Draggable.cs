using UnityEngine;
using AutoBattler.Units;
using AutoBattler.EventManagers;

public class Draggable : MonoBehaviour
{
    [SerializeField] private int dragSortingOrder = 10;
    [SerializeField] private Vector3 dragOffset = new Vector3(-0.1f, -0.1f, 0);

    private SpriteRenderer spriteRenderer;
    private BaseUnit unit;
    private Camera mainCamera;

    private Vector3 startPosition;
    private int startSortingOrder;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        unit = GetComponent<BaseUnit>();
        mainCamera = Camera.main;
    }

    public void OnStartDrag()
    {
        startPosition = transform.position;
        startSortingOrder = spriteRenderer.sortingOrder;
        spriteRenderer.sortingOrder = dragSortingOrder;
    }

    public void OnDragging()
    {
        Vector3 currentPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        currentPosition.z = 0;

        UnitsEventManager.SendDraggedUnitChangedPosition(currentPosition);

        currentPosition += dragOffset;
        transform.position = currentPosition;
    }

    public void OnEndDrag()
    {
        Vector3 currentPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        transform.position = startPosition;
        spriteRenderer.sortingOrder = startSortingOrder;

        UnitsEventManager.SendUnitEndDrag(unit, currentPosition);
    }
}
