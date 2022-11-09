using System.Collections;
using UnityEngine;
using TMPro;
using AutoBattler.Managers;

namespace AutoBattler.UI.ResultNotifications
{
    public class RoundWonNotification : RoundResultNotification
    {
        [Header("Components")]
        [SerializeField] private TextMeshProUGUI textGold;

        public void Setup(int goldAmount)
        {
            textGold.text = GetCharacteristicStr(goldAmount);
        }
    }
}
