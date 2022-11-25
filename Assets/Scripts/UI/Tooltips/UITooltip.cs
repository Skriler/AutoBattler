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
            CalculatePosition();
        }

        private void SetScreenParameters()
        {
            UICanvasSize = new Vector2(
                UICanvasRectTransform.rect.width,
                UICanvasRectTransform.rect.height
                );
            screenSize = new Vector2(Screen.width, Screen.height);
        }

        private void CalculatePosition()
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
            tooltipSize = new Vector2(
                rectTransform.rect.width,
                rectTransform.rect.height
                );
            tooltipSize = GetActualSize(tooltipSize);

            if (position.x > screenSize.x - tooltipSize.x)
                position.x = screenSize.x - tooltipSize.x;

            if (position.y > screenSize.y - tooltipSize.y)
                position.y = screenSize.y - tooltipSize.y;

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
            CalculatePosition();
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }
    }
}
