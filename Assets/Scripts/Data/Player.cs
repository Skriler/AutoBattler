using UnityEngine;
using AutoBattler.UnitBoxes;

public class Player : MonoBehaviour
{
    [SerializeField] private int startHealth = 50;
    [SerializeField] private int startGold = 0;

    public StorageManager Storage { get; private set; }
    public FieldManager Field { get; private set; }
    public int Health { get; private set; }
    public int Gold { get; private set; }

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
        Storage = transform.GetComponentInChildren<StorageManager>();
        Field = transform.GetComponentInChildren<FieldManager>();

        UIEventManager.SendGoldAmountChanged(Gold);
        UIEventManager.SendHealthAmountChanged(Health);
    }

    public bool IsEnoughGoldForAction(int actionCost) => Gold >= actionCost;

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
}
