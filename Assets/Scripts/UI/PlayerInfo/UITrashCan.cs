using UnityEngine;
using UnityEngine.UI;
using AutoBattler.Data.Units;
using AutoBattler.EventManagers;
using AutoBattler.Managers;

namespace AutoBattler.UI.PlayerInfo
{
    public class UITrashCan : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Image trashCanImage;
        [SerializeField] private Sprite trashCanSprite;
        [SerializeField] private Sprite openedTrashCanSprite;

        [Header("Sounds")]
        [SerializeField] private AudioSource sellUnitSound;

        private Camera mainCamera;
        private Vector3 position;
        private Vector3 size;

        public bool IsOpened { get; private set; } = false;

        private void Awake()
        {
            UnitsEventManager.OnDraggedUnitChangedPosition += CheckUnitPosition;
            UnitsEventManager.OnUnitEndDrag += SellUnit;
        }

        private void OnDestroy()
        {
            UnitsEventManager.OnDraggedUnitChangedPosition += CheckUnitPosition;
            UnitsEventManager.OnUnitEndDrag -= SellUnit;
        }

        private void Start()
        {
            RectTransform trashCanRectTransform = gameObject.GetComponent<RectTransform>();

            mainCamera = Camera.main;
            size = trashCanRectTransform.sizeDelta;
            position = transform.position;
        }

        private void ChangeImageSprite(Sprite sprite) => trashCanImage.sprite = sprite;

        private void CheckUnitPosition(Vector3 worldPosition)
        {
            Vector3 canvasPosition = RectTransformUtility.WorldToScreenPoint(mainCamera, worldPosition);

            if (IsPositionInTrashCan(canvasPosition))
            {
                if (!IsOpened)
                {
                    AudioManager.Instance.PlayHoverSound();
                    ChangeImageSprite(openedTrashCanSprite);
                    IsOpened = true;
                }
            }
            else
            {
                if (IsOpened)
                {
                    ChangeImageSprite(trashCanSprite);
                    IsOpened = false;
                }
            }
        }

        private bool IsPositionInTrashCan(Vector3 position)
        {
            if (position.x < this.position.x - this.size.x / 2)
                return false;

            if (position.x > this.position.x + this.size.x / 2)
                return false;

            if (position.y < this.position.y - this.size.y / 2)
                return false;

            if (position.y > this.position.y + this.size.y / 2)
                return false;

            return true;
        }

        private void SellUnit(BaseUnit unit, Vector3 worldPosition)
        {
            Vector3 canvasPosition = RectTransformUtility.WorldToScreenPoint(mainCamera, worldPosition);

            if (IsPositionInTrashCan(canvasPosition))
            {
                sellUnitSound?.Play();
                Destroy(unit.gameObject);
                UnitsEventManager.SendUnitSold(unit);
            }

            ChangeImageSprite(trashCanSprite);
            IsOpened = false;
        }
    }
}
