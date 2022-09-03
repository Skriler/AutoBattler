using UnityEngine;
using TMPro;
using AutoBattler.EventManagers;

namespace AutoBattler.UI
{
    public class UIPlayerInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textGold;
        [SerializeField] private TextMeshProUGUI textHealth;
        [SerializeField] private TextMeshProUGUI textTavernTier;

        private void OnEnable()
        {
            UIEventManager.OnGoldAmountChanged += SetGold;
            UIEventManager.OnHealthAmountChanged += SetHealth;
            UIEventManager.OnTavernTierChanged += SetTavernTier;
        }

        private void OnDestroy()
        {
            UIEventManager.OnGoldAmountChanged -= SetGold;
            UIEventManager.OnHealthAmountChanged -= SetHealth;
            UIEventManager.OnTavernTierChanged -= SetTavernTier;
        }

        private void SetGold(int gold)
        {
            textGold.text = gold.ToString();
        }

        private void SetHealth(int health)
        {
            textHealth.text = health.ToString();
        }

        private void SetTavernTier(int tavernTier)
        {
            textTavernTier.text = tavernTier.ToString();
        }
    }
}
