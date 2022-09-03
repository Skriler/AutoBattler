using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace AutoBattler.UI
{
    public class UICard : MonoBehaviour
    {
        [SerializeField] private UnityEvent<UICard, ShopDatabase.ShopUnit> OnCardClick;

        [SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField] private TextMeshProUGUI textCost;
        [SerializeField] private Image unitImage;
        [SerializeField] private float swapSpeed = 0.4f;

        private ShopDatabase.ShopUnit shopUnit;
        private Sprite[] unitSprites;
        private int spriteIndex = 0;

        public Image backgroundImage { get; private set; }

        public void OnClick() => OnCardClick.Invoke(this, shopUnit);

        public void MouseOver() => UICardTooltip.Show();

        public void MouseExit() => UICardTooltip.Hide();

        public void Setup(ShopDatabase.ShopUnit shopUnit)
        {
            if (IsInvoking("SwapSprite"))
                CancelInvoke("SwapSprite");

            spriteIndex = 0;
            textTitle.text = shopUnit.title;
            textCost.text = shopUnit.cost.ToString();
            unitSprites = shopUnit.sprites;

            unitImage.sprite = unitSprites[spriteIndex];
            this.shopUnit = shopUnit;

            InvokeRepeating("SwapSprite", swapSpeed, swapSpeed);
        }

        private void SwapSprite()
        {
            spriteIndex = (spriteIndex < unitSprites.Length) ? spriteIndex : 0;
            unitImage.sprite = unitSprites[spriteIndex];
            spriteIndex++;
        }
    }
}
