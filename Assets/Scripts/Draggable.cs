using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    [SerializeField] private int dragSortingOrder = 10;
    [SerializeField] private Vector3 dragOffset = new Vector3(-0.1f, -0.1f, 0);

    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;

    private Vector3 startPosition;
    private int startSortingOrder;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        Vector3 currentPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition) + dragOffset;

        EventManager.SendDraggedUnitChangedPosition(currentPosition);

        currentPosition.z = 0;
        transform.position = currentPosition;
        currentPosition += dragOffset;
    }

    public void OnEndDrag()
    {
        Vector3 currentPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        EventManager.SendUnitEndDrag(currentPosition);

        transform.position = startPosition;
        spriteRenderer.sortingOrder = startSortingOrder;
    }
}
