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
        [SerializeField] private float swapSpeed = 0.2f;

        private ShopDatabase.ShopUnit shopUnit;
        private Sprite[] unitSprites;
        private int spriteIndex = 0;

        public void Setup(ShopDatabase.ShopUnit shopUnit)
        {
            textTitle.text = shopUnit.title;
            textCost.text = shopUnit.cost.ToString();
            //unitImage.sprite = unitSprites[spriteIndex];
            this.shopUnit = shopUnit;

            //InvokeRepeating("SwapSprite", swapSpeed, swapSpeed);
        }

        public void OnClick()
        {
            OnCardClick.Invoke(this, shopUnit);
        }

        //private void SwapSprite()
        //{
        //    spriteIndex = (spriteIndex < unitSprites.Length) ? spriteIndex : 0;
        //    unitImage.sprite = unitSprites[spriteIndex];
        //    spriteIndex++;
        //}
    }
}
