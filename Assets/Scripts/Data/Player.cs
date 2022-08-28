using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int startHealth = 50; 
    [SerializeField] private int startGold = 0; 

    public Army Field { get; private set; }
    public Storage Storage { get; private set; }
    public int Health { get; private set; }
    public int Gold { get; private set; }

    private void Start()
    {
        Health = startHealth;
        Gold = startGold;

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
}
