using UnityEngine;
using AutoBattler.UnitsContainers.Containers;
using AutoBattler.Data.Units;
using AutoBattler.EventManagers;
using AutoBattler.Data.ScriptableObjects.Characteristics;
using AutoBattler.Managers;

namespace AutoBattler.Data.Players
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerCharacteristics characteristics;

        public StorageContainer Storage { get; private set; }
        public PlayerFieldContainer Field { get; private set; }
        public EnemyFieldContainer EnemyField { get; private set; }
        public int Health { get; private set; }
        public int Gold { get; private set; }
        public int TavernTier { get; private set; }

        private void Awake()
        {
            UnitsEventManager.OnUnitSold += SellUnit;

            SetStartPlayerCharacteristics();
        }

        private void OnDestroy()
        {
            UnitsEventManager.OnUnitSold -= SellUnit;
        }

        private void Start()
        {
            Storage = transform.GetComponentInChildren<StorageContainer>();
            Field = transform.Find("PlayerField").GetComponent<PlayerFieldContainer>();
            EnemyField = transform.Find("EnemyField").GetComponent<EnemyFieldContainer>();

            PlayerEventManager.SendGoldAmountChanged(Gold);
            PlayerEventManager.SendHealthAmountChanged(Health);
            PlayerEventManager.SendTavernTierChanged(TavernTier);
        }

        private void SetStartPlayerCharacteristics()
        {
            Health = characteristics.StartHealth;
            Gold = characteristics.StartGold;
            TavernTier = characteristics.StartTavernTier;
        }

        public bool IsEnoughGoldForAction(int actionCost) => Gold >= actionCost;

        public bool IsMaxTavernTier() => TavernTier >= characteristics.MaxTavernTier;

        public bool IsAlive() => Health > 0;

        public void SpendGold(int actionCost)
        {
            if (Gold - actionCost < 0)
            {
                Debug.Log("Not enough gold");
                return;
            }

            Gold -= actionCost;
            PlayerEventManager.SendGoldAmountChanged(Gold);
        }

        public void SellUnit(BaseUnit unit)
        {
            GainGold(1);
        }

        public void GainGold(int gold)
        {
            Gold += gold;
            Gold = Gold > characteristics.MaxGold ? characteristics.MaxGold : Gold;

            PlayerEventManager.SendGoldAmountChanged(Gold);
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            Health = Health < 0 ? 0 : Health;

            PlayerEventManager.OnHealthAmountChanged(Health);

            if (!IsAlive())
                Death();
        }

        public void LevelUpTavernTier()
        {
            if (TavernTier >= characteristics.MaxTavernTier)
                return;

            if (!IsEnoughGoldForAction(characteristics.LevelUpTavernTierCost))
                return;

            SpendGold(characteristics.LevelUpTavernTierCost);
            ++TavernTier;
            PlayerEventManager.OnTavernTierChanged(TavernTier);
        }

        public void Death()
        {
            Destroy(this.gameObject);
        }
    }
}