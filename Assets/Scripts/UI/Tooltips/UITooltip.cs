using UnityEngine;

namespace AutoBattler.UI.Tooltips
{
    public abstract class UITooltip : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] protected Vector2 offset = new Vector2(160, -160);

        protected RectTransform backgroundRectTransform;
        protected Camera camera;

        protected void Start()
        {
            backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
            camera = Camera.current;
            gameObject.SetActive(false);
        }

        protected void Update()
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform.parent.GetComponent<RectTransform>(),
                Input.mousePosition,
                camera,
                out localPoint);

            transform.localPosition = localPoint + offset;
        }

        public abstract void Setup(Object data);

        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);
    }
}
