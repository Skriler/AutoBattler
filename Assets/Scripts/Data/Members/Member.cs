﻿using System;
using UnityEngine;
using AutoBattler.Data.Units;
using AutoBattler.UnitsContainers.Containers.Field;
using AutoBattler.Data.ScriptableObjects.Characteristics;
using AutoBattler.Managers;

namespace AutoBattler.Data.Members
{
    public abstract class Member : MonoBehaviour
    {
        [SerializeField] protected MemberCharacteristics characteristics;

        public EnemyFieldContainer EnemyField { get; protected set; }
        public string Id { get; protected set; }
        public int Health { get; protected set; }
        public int Gold { get; protected set; }
        public int TavernTier { get; protected set; }
        public int RoundsWonAmount { get; protected set; }
        public int LevelUpTavernTierCost { get; protected set; } = 4;

        protected int startGainGoldPerRound;
        protected int maxGainGoldPerRound;

        protected virtual void Awake()
        {
            SetStartPlayerCharacteristics();
        }

        protected virtual void Start()
        {
            EnemyField = transform.GetComponentInChildren<EnemyFieldContainer>();
            Id = Guid.NewGuid().ToString("N");

            startGainGoldPerRound = GameManager.Instance.StartGainGoldPerRound;
            maxGainGoldPerRound = GameManager.Instance.MaxGainGoldPerRound;
        }

        public abstract FieldContainer GetFieldContainer();
        public abstract FieldContainer GetEnemyFieldContainer();

        public bool IsEnoughGoldForAction(int actionCost) => Gold >= actionCost;

        public bool IsMaxTavernTier() => TavernTier >= characteristics.MaxTavernTier;

        public bool IsAlive() => Health > 0;

        protected void SetStartPlayerCharacteristics()
        {
            Health = characteristics.StartHealth;
            Gold = characteristics.StartGold;
            TavernTier = characteristics.StartTavernTier;
        }

        public int GetRoundRewardGoldAmount()
        {
            int gainGold = startGainGoldPerRound + GameManager.Instance.CurrentRound;

            return gainGold <= maxGainGoldPerRound ? gainGold : maxGainGoldPerRound;
        }
    }
}