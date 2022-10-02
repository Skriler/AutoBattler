using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using AutoBattler.Data.ScriptableObjects.Databases;
using AutoBattler.UI.Tooltips;

namespace AutoBattler.UI.Shop
{
    public class UICard : MonoBehaviour
    {
        private static float MAX_COLOR_VALUE = 255;

        [Header("Events")]
        [SerializeField] private UnityEvent<UICard, ShopUnitEntity> OnCardClick;

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField] private TextMeshProUGUI textCost;
        [SerializeField] private Image unitImage;

        [Header("Parameters")]
        [SerializeField] private int freezeColorR = 100;
        [SerializeField] private int freezeColorG = 170;
        [SerializeField] private int freezeColorB = 250;

        private ShopUnitEntity shopUnit;
        private Sprite[] unitSprites;
        private int currentSpriteIndex = 0;

        public bool IsFreezed { get; private set; }

        public void OnClick() => OnCardClick.Invoke(this, shopUnit);

        public void MouseExit() => UIShopUnitTooltip.Instance.Hide();

        public void MouseEnter()
        {
            UIShopUnitTooltip.Instance.Show();
            UIShopUnitTooltip.Instance.Setup(shopUnit.characteristics);
        }

        public void Setup(ShopUnitEntity shopUnit)
        {
            if (IsInvoking("SwapSprite"))
                CancelInvoke("SwapSprite");

            textTitle.text = shopUnit.characteristics.Title;
            textCost.text = shopUnit.characteristics.Cost.ToString();
            unitSprites = shopUnit.sprites;
            this.shopUnit = shopUnit;

            int yAngle = shopUnit.isFlipOnX ? 180 : 0;
            unitImage.transform.rotation = Quaternion.Euler(0, yAngle, 0); 

            currentSpriteIndex = 0;
            unitImage.sprite = unitSprites[currentSpriteIndex];
            InvokeRepeating("SwapSprite", shopUnit.swapSpeed, shopUnit.swapSpeed);
        }

        public void Freeze()
        {
            Color cardColor = Color.white;

            if (!IsFreezed)
            {
                cardColor.r = freezeColorR / MAX_COLOR_VALUE;
                cardColor.g = freezeColorG / MAX_COLOR_VALUE;
                cardColor.b = freezeColorB / MAX_COLOR_VALUE;
            }

            unitImage.color = cardColor;

            IsFreezed = !IsFreezed;
        }

        private void SwapSprite()
        {
            currentSpriteIndex = (currentSpriteIndex < unitSprites.Length) ? currentSpriteIndex : 0;
            unitImage.sprite = unitSprites[currentSpriteIndex];
            ++currentSpriteIndex;
        }
    }
}
