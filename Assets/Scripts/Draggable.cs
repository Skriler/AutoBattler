using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    [SerializeField] private int dragSortingOrder = 10;
    [SerializeField] private Vector3 dragOffset = new Vector3(-0.1f, -0.1f, 0);

    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;

    private Vector3 startPosition;
    private int startSortingOrder;

    public void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnStartDrag()
    {
        Debug.Log("OnStartDrag");

        startPosition = transform.position;
        startSortingOrder = spriteRenderer.sortingOrder;
        spriteRenderer.sortingOrder = dragSortingOrder;
    }

    public void OnDragging()
    {
        Debug.Log("OnDragging");

        Vector3 currentPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition) + dragOffset;
        currentPosition.z = 0;
        transform.position = currentPosition;
    }

    public void OnEndDrag()
    {
        Debug.Log("OnEndDrag");

        transform.position = startPosition;
        spriteRenderer.sortingOrder = startSortingOrder;
    }
}
