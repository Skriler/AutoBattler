using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using AutoBattler.Data.ScriptableObjects;

namespace AutoBattler.UI
{
    public class UICard : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private UnityEvent<UICard, ShopDatabase.ShopUnit> OnCardClick;

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField] private TextMeshProUGUI textCost;
        [SerializeField] private Image unitImage;

        private ShopDatabase.ShopUnit shopUnit;
        private Sprite[] unitSprites;
        private int currentSpriteIndex = 0;

        public Image backgroundImage { get; private set; }

        public void OnClick() => OnCardClick.Invoke(this, shopUnit);

        public void MouseExit() => UICardTooltip.Instance.Hide();

        public void MouseOver()
        {
            UICardTooltip.Instance.Show();
            UICardTooltip.Instance.Setup(shopUnit.characteristics);
        }

        public void Setup(ShopDatabase.ShopUnit shopUnit)
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

        private void SwapSprite()
        {
            currentSpriteIndex = (currentSpriteIndex < unitSprites.Length) ? currentSpriteIndex : 0;
            unitImage.sprite = unitSprites[currentSpriteIndex];
            ++currentSpriteIndex;
        }
    }
}
