using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace AutoBattler.UI
{
    public class UIPlayerInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textGold;
        [SerializeField] private TextMeshProUGUI textHealth;

        private void OnEnable()
        {
            UIEventManager.OnGoldAmountChanged += SetGold;
            UIEventManager.OnHealthAmountChanged += SetHealth;
        }

        private void OnDestroy()
        {
            UIEventManager.OnGoldAmountChanged -= SetGold;
            UIEventManager.OnHealthAmountChanged -= SetHealth;
        }

        private void SetGold(int gold)
        {
            textGold.text = "Gold: " + gold.ToString();
        }

        private void SetHealth(int health)
        {
            textHealth.text = "Health: " + health.ToString();
        }
    }
}
