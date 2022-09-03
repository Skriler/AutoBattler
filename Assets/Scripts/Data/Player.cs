using UnityEngine;
using AutoBattler.UnitsContainers.Containers;
using AutoBattler.Units;
using AutoBattler.EventManagers;

public class Player : MonoBehaviour
{
    [SerializeField] private int startHealth = 50;
    [SerializeField] private int startGold = 0;
    [SerializeField] private int startTavernTier = 1;
    [SerializeField] private int maxTavernTier = 5;
    [SerializeField] private int levelUpTavernTierCost = 5;

    public StorageContainer Storage { get; private set; }
    public FieldContainer Field { get; private set; }
    public int Health { get; private set; }
    public int Gold { get; private set; }
    public int TavernTier { get; private set; }

    private void OnEnable()
    {
        UnitsEventManager.OnUnitSold += SellUnit;
    }

    private void OnDestroy()
    {
        UnitsEventManager.OnUnitSold -= SellUnit;
    }

    private void Start()
    {
        Health = startHealth;
        Gold = startGold;
        TavernTier = startTavernTier;

        Storage = transform.GetComponentInChildren<StorageContainer>();
        Field = transform.GetComponentInChildren<FieldContainer>();

        UIEventManager.SendGoldAmountChanged(Gold);
        UIEventManager.SendHealthAmountChanged(Health);
        UIEventManager.SendTavernTierChanged(TavernTier);
    }

    public bool IsEnoughGoldForAction(int actionCost) => Gold >= actionCost;

    public bool IsMaxTavernTier() => TavernTier >= maxTavernTier;

    public void SpendGold(int actionCost)
    {
        if (Gold - actionCost < 0)
        {
            Debug.Log("Not enough gold");
            return;
        }

        Gold -= actionCost;
        UIEventManager.SendGoldAmountChanged(Gold);
    }

    public void SellUnit(BaseUnit unit)
    {
        Gold += unit.Cost;
        UIEventManager.SendGoldAmountChanged(Gold);
    }

    public void LevelUpTavernTier()
    {
        if (TavernTier >= maxTavernTier)
            return;

        if (!IsEnoughGoldForAction(levelUpTavernTierCost))
            return;

        SpendGold(levelUpTavernTierCost);
        ++TavernTier;
        UIEventManager.OnTavernTierChanged(TavernTier);
    }
}
