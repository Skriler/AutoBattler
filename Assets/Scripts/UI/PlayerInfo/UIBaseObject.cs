using UnityEngine;
using AutoBattler.UI.Tooltips;
using AutoBattler.Data.ScriptableObjects.Characteristics;

namespace AutoBattler.UI.PlayerInfo
{
    public class UIBaseObject : MonoBehaviour
    {
        [SerializeField] private UIBaseObjectCharacteristics characteristics;

        public void MouseExit() => UIBaseObjectTooltip.Instance.Hide();

        public void MouseEnter()
        {
            UIBaseObjectTooltip.Instance.Show();
            UIBaseObjectTooltip.Instance.Setup(characteristics);
        }
    }
}
