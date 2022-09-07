using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler.Data.Buffs
{
    public enum BuffLevel
    {
        None,
        First,
        Second,
        Third
    }

    public abstract class BaseBuff
    {
        [SerializeField] private string title;
        [SerializeField] private BuffLevel maxBuffLevel;

        public bool IsActive { get; private set; }
        public BuffLevel CurrentBuffLevel { get; private set; }
    }
}