using UnityEngine;
using TMPro;

namespace AutoBattler.UI.ResultNotifications
{
    public class RoundWonNotification : RoundResultNotification
    {
        [Header("Components")]
        [SerializeField] private TextMeshProUGUI textGold;
        [SerializeField] private TextMeshProUGUI textGoldenCup;
        [SerializeField] private GameObject goldenCupContainer;

        public void Setup(int goldAmount, int goldenCupAmount)
        {
            textGold.text = GetCharacteristicStr(goldAmount);
            textGoldenCup.text = GetCharacteristicStr(goldenCupAmount);
        }

        public void SetActiveGoldenCupContainer(bool value) => goldenCupContainer.SetActive(value);
    }
}
