using UnityEngine;
using AutoBattler.EventManagers;

namespace AutoBattler.UI.Tooltips
{
    public abstract class UITooltip : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private RectTransform UICanvasRectTransform;

        [Header("Parameters")]
        [SerializeField] protected Vector2 offset = new Vector2(160, -160);
        [SerializeField] protected bool isPivotOnTop = false;

        protected RectTransform rectTransform;
        protected Camera currentCamera;

        private Vector2 UICanvasSize;
        private Vector2 screenSize;
        private Vector2 tooltipSize;

        public abstract void Setup(Object data);

        protected virtual void Awake()
        {
            UIEventManager.OnScreenSizeChanged += SetScreenParameters;
        }

        protected void OnDestroy()
        {
            UIEventManager.OnScreenSizeChanged -= SetScreenParameters;
        }

        protected virtual void Start()
        {
            rectTransform = transform.GetComponent<RectTransform>();
            currentCamera = Camera.current;

            gameObject.SetActive(false);
        }

        protected void OnEnable()
        {
            SetScreenParameters();
        }

        protected void Update()
        {
            CalculateAndSetPosition();
        }

        private void SetScreenParameters()
        {
            UICanvasSize = new Vector2(
                UICanvasRectTransform.rect.width,
                UICanvasRectTransform.rect.height
                );
            screenSize = new Vector2(Screen.width, Screen.height);
        }

        protected virtual void CalculateAndSetPosition()
        {
            Vector2 mousePositon = Input.mousePosition;
            mousePositon += GetActualSize(offset);

            mousePositon = ClampPosition(mousePositon);

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform.parent.GetComponent<RectTransform>(),
                mousePositon,
                currentCamera,
                out localPoint);

            transform.localPosition = localPoint;
        }

        private Vector2 ClampPosition(Vector2 position)
        {
            Vector2 mousePosition = Input.mousePosition;

            tooltipSize = new Vector2(
                rectTransform.rect.width,
                rectTransform.rect.height
                );
            tooltipSize = GetActualSize(tooltipSize);

            if (position.x > screenSize.x - tooltipSize.x)
                position.x = screenSize.x - tooltipSize.x;

            if (position.y > screenSize.y - tooltipSize.y && !isPivotOnTop)
                position.y = screenSize.y - tooltipSize.y;

            if (position.x <= mousePosition.x && position.x + tooltipSize.x >= mousePosition.x &&
                position.y <= mousePosition.y && position.y + tooltipSize.y >= mousePosition.y)
            {
                position.y -= tooltipSize.y;
            }    

            return position;
        }

        private Vector2 GetActualSize(Vector2 size)
        {
            size.x *= screenSize.x / UICanvasSize.x;
            size.y *= screenSize.y / UICanvasSize.y;

            return size;
        }

        public void Hide() => gameObject.SetActive(false);

        public void Show()
        {
            CalculateAndSetPosition();
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }
    }
}
