using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer backgroundSpriteRenderer;

    [Header("Parameters")]
    [SerializeField] private float zoomStep = 0.5f;
    [SerializeField] private float minCameraSize = 2;
    [SerializeField] private float maxCameraSize = 6;

    private Camera mainCamera;
    private Vector3 startPosition;
    private float backgroundMinX, backgroundMaxX;
    private float backgroundMinY, backgroundMaxY;

    private void Awake()
    {
        CalculateBackgroundParameters();
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsCameraMovementActive)
            return;

        PanCamera();
    }

    public void ZoomIn()
    {
        float newCameraSize = mainCamera.orthographicSize - zoomStep;
        mainCamera.orthographicSize = Mathf.Clamp(newCameraSize, minCameraSize, maxCameraSize);

        mainCamera.transform.position = ClampCamera(mainCamera.transform.position);
    }

    public void ZoomOut()
    {
        float newCameraSize = mainCamera.orthographicSize + zoomStep;
        mainCamera.orthographicSize = Mathf.Clamp(newCameraSize, minCameraSize, maxCameraSize);

        mainCamera.transform.position = ClampCamera(mainCamera.transform.position);
    }

    private void CalculateBackgroundParameters()
    {
        Vector3 position = backgroundSpriteRenderer.transform.position;
        Vector3 size = backgroundSpriteRenderer.bounds.size;

        backgroundMinX = position.x - size.x / 2f;
        backgroundMaxX = position.x + size.x / 2f;
        backgroundMinY = position.y - size.y / 2f;
        backgroundMaxY = position.y + size.y / 2f;
    }

    private void PanCamera()
    {
        if (Input.GetMouseButtonDown(0))
            startPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = startPosition - mainCamera.ScreenToWorldPoint(Input.mousePosition);

            mainCamera.transform.position = ClampCamera(mainCamera.transform.position + difference);
        }
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = mainCamera.orthographicSize;
        float camWidth = mainCamera.orthographicSize * mainCamera.aspect;

        float minX = backgroundMinX + camWidth;
        float maxX = backgroundMaxX - camWidth;
        float minY = backgroundMinY + camHeight;
        float maxY = backgroundMaxY - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY, targetPosition.z);
    }
}
