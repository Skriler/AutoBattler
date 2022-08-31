using UnityEngine;

public class UICardTooltip : MonoBehaviour
{
    private static UICardTooltip Instance;
    private RectTransform backgroundRectTransform;
    private Camera camera;

    private void Awake()
    {
        Instance = this;
        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent.GetComponent<RectTransform>(), 
            Input.mousePosition, 
            camera, 
            out localPoint);

        transform.localPosition = localPoint;
    }

    public static void ShowTooltip()
    {
        Instance.gameObject.SetActive(true);
    }

    public static void HideTooltip()
    {
        Instance.gameObject.SetActive(false);
    }
}
