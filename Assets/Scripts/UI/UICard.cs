using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace AutoBattler.UI
{
    public class UICard : MonoBehaviour
    {
        [SerializeField] private UnityEvent<UICard, ShopDatabase.ShopUnit> OnCardClick;

        [SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField] private TextMeshProUGUI textCost;
        [SerializeField] private Image unitImage;
        //public GameObject prefab;
        //public GameObject unitCard;

        private ShopDatabase.ShopUnit shopUnit;

        public void Setup(ShopDatabase.ShopUnit shopUnit)
        {
            textTitle.text = shopUnit.title;
            textCost.text = shopUnit.cost.ToString();
            unitImage.sprite = shopUnit.image;

            this.shopUnit = shopUnit;

            //shopUnit.prefab.transform.position = prefab.transform.position;
            //prefab = shopUnit.prefab;

            //GameObject unit = Instantiate(prefab, prefab.transform.position, Quaternion.identity);
            //unit.transform.SetParent(unitCard.transform);
            //unit.transform.localScale = new Vector2(120, 120);
            //unit.transform.SetAsLastSibling();
        }

        public void OnClick()
        {
            OnCardClick.Invoke(this, shopUnit);
        }
    }
}
