using UnityEngine;
using TMPro;
using AutoBattler.UI.Tooltips;
using AutoBattler.Data.Members;

namespace AutoBattler.UI.PlayerInfo
{
    public class UIMemberHealth : UIBaseObject
    {
        [Header("Components")]
        [SerializeField] protected Member owner;
        [SerializeField] protected TextMeshProUGUI textHealth;

        private Camera mainCamera;
        private Vector3 ownerPosition;

        private string startTitle;
        private string startDescription;

        public Member Owner => owner;

        protected override void Awake()
        {
            base.Awake();

            startTitle = Title;
            startDescription = Description;
        }

        protected void Start()
        {
            mainCamera = Camera.main;
            ownerPosition = owner.gameObject.transform.position;

            UpdateTitle();
            UpdateDescription();
            UpdateHealth();
        }

        public void UpdateTitle()
        {
            Title = Owner.Id + " " + startTitle;
        }

        public void UpdateDescription()
        {
            Description = startDescription + owner?.Health;
            UIBaseObjectTooltip.Instance.Setup(this);
        }

        public void UpdateHealth()
        {
            textHealth.text = owner?.Health.ToString();
        }

        public void SetCameraOnOwnerField()
        {
            ownerPosition.z = mainCamera.transform.position.z;

            mainCamera.transform.position = ownerPosition;
        }
    }
}
