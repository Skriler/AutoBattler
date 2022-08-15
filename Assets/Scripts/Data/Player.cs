using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int startHealth = 50; 
    [SerializeField] private int startGold = 50; 

    public Army Field { get; private set; }
    public Storage Storage { get; private set; }
    public int Health { get; private set; }
    public int Gold { get; private set; }

    private void Start()
    {
        Health = startHealth;
        Gold = startGold;
    }
}
