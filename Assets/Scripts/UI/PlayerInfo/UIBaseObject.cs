using UnityEngine;
using AutoBattler.UI.Tooltips;
using AutoBattler.Data.ScriptableObjects.Characteristics;

namespace AutoBattler.UI.PlayerInfo
{
    public class UIBaseObject : MonoBehaviour
    {
        [SerializeField] protected UIBaseObjectCharacteristics characteristics;

        public string Title { get; protected set; }
        public string Description { get; protected set; }

        protected virtual void Start()
        {
            Title = characteristics.Title;
            Description = characteristics.Description;
        }

        public void MouseExit() => UIBaseObjectTooltip.Instance.Hide();

        public void MouseEnter()
        {
            UIBaseObjectTooltip.Instance.Show();
            UIBaseObjectTooltip.Instance.Setup(this);
        }
    }
}
