using UnityEngine;
using AutoBattler.UnitsContainers.Containers;
using AutoBattler.Data.Units;
using AutoBattler.EventManagers;
using AutoBattler.Data.ScriptableObjects.Characteristics;
using AutoBattler.Managers;
using AutoBattler.SaveSystem;
using AutoBattler.SaveSystem.Data;

namespace AutoBattler.Data.Player
{
    public class Player : MonoBehaviour, IDataPersistence
    {
        [SerializeField] private PlayerCharacteristics characteristics;

        public StorageContainer Storage { get; private set; }
        public PlayerFieldContainer Field { get; private set; }
        public EnemyFieldContainer EnemyField { get; private set; }
        public int Health { get; private set; }
        public int Gold { get; private set; }
        public int TavernTier { get; private set; }
        public int RoundsWonAmount { get; private set; }
        public int LevelUpTavernTierCost { get; private set; } = 4;

        private int maxGainGoldPerRound;

        private void Awake()
        {
            UnitsEventManager.OnUnitSold += SellUnit;
            UnitsEventManager.OnUnitEndDrag += PlayDragSound;

            SetStartPlayerCharacteristics();
        }

        private void OnDestroy()
        {
            UnitsEventManager.OnUnitSold -= SellUnit;
            UnitsEventManager.OnUnitEndDrag -= PlayDragSound;
        }

        private void Start()
        {
            Storage = transform.GetComponentInChildren<StorageContainer>();
            Field = transform.Find("PlayerField").GetComponent<PlayerFieldContainer>();
            EnemyField = transform.Find("EnemyField").GetComponent<EnemyFieldContainer>();

            maxGainGoldPerRound = GameManager.Instance.MaxGainGoldPerRound;

            PlayerEventManager.SendGoldAmountChanged(Gold);
            PlayerEventManager.SendHealthAmountChanged(Health);
            PlayerEventManager.SendTavernTierIncreased(TavernTier);
        }

        public bool IsEnoughGoldForAction(int actionCost) => Gold >= actionCost;

        public bool IsMaxTavernTier() => TavernTier >= characteristics.MaxTavernTier;

        public bool IsAlive() => Health > 0;

        private void SetStartPlayerCharacteristics()
        {
            Health = characteristics.StartHealth;
            Gold = characteristics.StartGold;
            TavernTier = characteristics.StartTavernTier;
        }

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

            PlayerEventManager.SendHealthAmountChanged(Health);

            if (!IsAlive())
                Death();
        }

        public void LevelUpTavernTier()
        {
            if (IsMaxTavernTier() || !IsEnoughGoldForAction(LevelUpTavernTierCost))
                return;

            SpendGold(LevelUpTavernTierCost);

            ++TavernTier;
            PlayerEventManager.OnTavernTierIncreased(TavernTier);
            LevelUpTavernTierCost = Field.GetOpenedCellsAmount();
        }

        public void IncreaseRoundsWonAmountByOne()
        {
            ++RoundsWonAmount;
            PlayerEventManager.SendRoundsWonAmountIncreased(RoundsWonAmount);
        }

        public void Death()
        {
            Destroy(this.gameObject);
        }

        private void PlayDragSound(BaseUnit unit, Vector3 worldPosition)
        {
            if (Storage.IsTileOnPosition(worldPosition) || Field.IsTileOnPosition(worldPosition))
                AudioManager.Instance.PlayUnitDragSuccessfulSound();
            else
                AudioManager.Instance.PlayUnitDragFailedSound();
        }

        public int GetRoundRewardGoldAmount()
        {
            int currentRound = GameManager.Instance.CurrentRound;

            return currentRound <= maxGainGoldPerRound ? currentRound : maxGainGoldPerRound;
        }

        public void LoadData(GameData data)
        {
            Health = data.health;
            Gold = data.gold;
            TavernTier = data.tavernTier;
            RoundsWonAmount = data.roundsWonAmount;
            LevelUpTavernTierCost = data.levelUpTavernTierCost;

            PlayerEventManager.SendGoldAmountChanged(Gold);
            PlayerEventManager.SendHealthAmountChanged(Health);
            PlayerEventManager.SendTavernTierIncreased(TavernTier);
            PlayerEventManager.SendRoundsWonAmountIncreased(RoundsWonAmount);
        }

        public void SaveData(GameData data)
        {
            data.health = Health;
            data.gold = Gold;
            data.tavernTier = TavernTier;
            data.roundsWonAmount = RoundsWonAmount;
            data.levelUpTavernTierCost = LevelUpTavernTierCost;
        }
    }
}