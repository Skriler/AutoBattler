using UnityEngine;
using TMPro;

namespace AutoBattler.UI.ResultNotifications
{
    public class RoundLostNotification : RoundResultNotification
    {
        [Header("Components")]
        [SerializeField] private TextMeshProUGUI textGold;
        [SerializeField] private TextMeshProUGUI textHealth;

        public void Setup(int goldAmount, int healthAmount)
        {
            textGold.text = GetCharacteristicStr(goldAmount);
            textHealth.text = GetCharacteristicStr(healthAmount);
        }
    }
}
