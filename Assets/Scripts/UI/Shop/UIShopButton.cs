using AutoBattler.UI.PlayerInfo;
using UnityEngine;

namespace AutoBattler.UI.Shop
{
    public class UIShopButton : UIBaseObject
    {
        [Header("Sounds")]
        [SerializeField] private AudioSource clickSound;

        public void PlayClickSound() => clickSound?.Play();
    }
}
