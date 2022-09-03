using UnityEngine;
using AutoBattler.UnitsContainers.Containers;
using AutoBattler.Units;
using AutoBattler.EventManagers;
using AutoBattler.Data.ScriptableObjects;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerCharacteristics characteristics;

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
        Health = characteristics.StartHealth;
        Gold = characteristics.StartGold;
        TavernTier = characteristics.StartTavernTier;

        Storage = transform.GetComponentInChildren<StorageContainer>();
        Field = transform.GetComponentInChildren<FieldContainer>();

        UIEventManager.SendGoldAmountChanged(Gold);
        UIEventManager.SendHealthAmountChanged(Health);
        UIEventManager.SendTavernTierChanged(TavernTier);
    }

    public bool IsEnoughGoldForAction(int actionCost) => Gold >= actionCost;

    public bool IsMaxTavernTier() => TavernTier >= characteristics.MaxTavernTier;

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
        if (TavernTier >= characteristics.MaxTavernTier)
            return;

        if (!IsEnoughGoldForAction(characteristics.LevelUpTavernTierCost))
            return;

        SpendGold(characteristics.LevelUpTavernTierCost);
        ++TavernTier;
        UIEventManager.OnTavernTierChanged(TavernTier);
    }
}
