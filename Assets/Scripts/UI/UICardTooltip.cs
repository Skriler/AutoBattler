using UnityEngine;
using TMPro;
using AutoBattler.Data.ScriptableObjects;

public class UICardTooltip : Manager<UICardTooltip>
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI textTitle;
    [SerializeField] private TextMeshProUGUI textCost;
    [SerializeField] private TextMeshProUGUI textTavernTier;
    [SerializeField] private TextMeshProUGUI textHealth;
    [SerializeField] private TextMeshProUGUI textAttackDamage;
    [SerializeField] private TextMeshProUGUI textAttackSpeed;

    [Header("Parameters")]
    [SerializeField] private Vector2 offset = new Vector2(160, -160);

    private RectTransform backgroundRectTransform;
    private Camera camera;

    private void Start()
    {
        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        camera = Camera.current;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent.GetComponent<RectTransform>(), 
            Input.mousePosition, 
            camera, 
            out localPoint);

        transform.localPosition = localPoint + offset;
    }

    public void Setup(UnitCharacteristics characteristics)
    {
        textTitle.text = characteristics.Title;
        textCost.text = "Cost: " + characteristics.Cost.ToString();
        textTavernTier.text = "Tier: " + characteristics.TavernTier.ToString();
        textHealth.text = "Health: " + characteristics.MaxHealth.ToString();
        textAttackDamage.text = "Damage: " + characteristics.AttackDamage.ToString();
        textAttackSpeed.text = "Speed: " + characteristics.AttackSpeed.ToString();
    }

    public void Show() => gameObject.SetActive(true);

    public void Hide() => gameObject.SetActive(false);
}
