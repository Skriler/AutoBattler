using UnityEngine;
using UnityEngine.UI;
using AutoBattler.Data.ScriptableObjects.Structs;

namespace AutoBattler.UI.Menu
{
    public class UIUnit : MonoBehaviour
    {
        private static int ANGEL_NORMAL = 0;
        private static int ANGLE_FLIP = 180;

        [Header("Components")]
        [SerializeField] private Image unitImage;

        [Header("Parameters")]
        [SerializeField] private bool isEnemy;

        private Sprite[] unitSprites;
        private int currentSpriteIndex = 0;

        public void Setup(ShopUnitEntity shopUnit)
        {
            if (IsInvoking("SwapSprite"))
                CancelInvoke("SwapSprite");

            unitSprites = shopUnit.sprites;

            int yAngle = shopUnit.isFlipOnX ? ANGLE_FLIP : ANGEL_NORMAL;

            if (isEnemy)
                yAngle = yAngle == ANGLE_FLIP ? ANGEL_NORMAL : ANGLE_FLIP;

            unitImage.transform.rotation = Quaternion.Euler(ANGEL_NORMAL, yAngle, ANGEL_NORMAL);

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
