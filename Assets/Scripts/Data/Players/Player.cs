using System.Collections.Generic;
using UnityEngine;
using AutoBattler.UnitsContainers.Containers;
using AutoBattler.Data.Units;
using AutoBattler.EventManagers;
using AutoBattler.Data.ScriptableObjects.Characteristics;
using AutoBattler.Data.Buffs;

namespace AutoBattler.Data.Players
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerCharacteristics characteristics;
        [SerializeField] private List<Buff> buffs;

        public StorageContainer Storage { get; private set; }
        public PlayerFieldContainer Field { get; private set; }
        public EnemyFieldContainer EnemyField { get; private set; }
        public BuffContainer Buffs { get; private set; }
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

        private void Awake()
        {
            SetStartPlayerCharacteristics();
        }

        private void Start()
        {
            Storage = transform.GetComponentInChildren<StorageContainer>();
            Field = transform.Find("PlayerField").GetComponent<PlayerFieldContainer>();
            EnemyField = transform.Find("EnemyField").GetComponent<EnemyFieldContainer>();
            Buffs = transform.GetComponentInChildren<BuffContainer>();


            UIEventManager.SendGoldAmountChanged(Gold);
            UIEventManager.SendHealthAmountChanged(Health);
            UIEventManager.SendTavernTierChanged(TavernTier);
        }

        private void SetStartPlayerCharacteristics()
        {
            Health = characteristics.StartHealth;
            Gold = characteristics.StartGold;
            TavernTier = characteristics.StartTavernTier;
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
            ++Gold;
            Gold = Gold > characteristics.MaxGold ? characteristics.MaxGold : Gold;

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
}