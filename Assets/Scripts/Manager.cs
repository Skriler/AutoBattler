using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager<T> : MonoBehaviour where T : Manager<T>
{
    public static T Instance { get; private set; }

    protected void Awake()
    {
        Instance = (T)this;
    }
}
