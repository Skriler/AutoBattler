using UnityEngine;
using AutoBattler.UnitsContainers.Containers.Field;
using AutoBattler.UnitsContainers.Containers.Storage;
using AutoBattler.Data.Units;
using AutoBattler.EventManagers;
using AutoBattler.Managers;
using AutoBattler.SaveSystem.Data;

namespace AutoBattler.Data.Members
{
    public class Player : Member
    {
        public PlayerStorageContainer Storage { get; protected set; }
        public PlayerFieldContainer Field { get; protected set; }

        protected override void Awake()
        {
            base.Awake();

            UnitsEventManager.OnUnitSold += SellUnit;
            UnitsEventManager.OnUnitEndDrag += PlayDragSound;
        }

        private void OnDestroy()
        {
            UnitsEventManager.OnUnitSold -= SellUnit;
            UnitsEventManager.OnUnitEndDrag -= PlayDragSound;
        }

        protected override void Start()
        {
            base.Start();

            Storage = transform.GetComponentInChildren<PlayerStorageContainer>();
            Field = transform.GetComponentInChildren<PlayerFieldContainer>();

            PlayerEventManager.SendGoldAmountChanged(Gold);
            PlayerEventManager.SendHealthAmountChanged(Health);
            PlayerEventManager.SendTavernTierIncreased(TavernTier);
        }

        public override MemberFieldContainer GetFieldContainer() => Field;

        public override void SpendGold(int actionCost)
        {
            base.SpendGold(actionCost);
            PlayerEventManager.SendGoldAmountChanged(Gold);
            ScoreManager.Instance.IncreaseScore(actionCost);
        }

        public override void GainGold(int gold)
        {
            base.GainGold(gold);
            PlayerEventManager.SendGoldAmountChanged(Gold);
            ScoreManager.Instance.IncreaseScore(gold);
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            PlayerEventManager.SendHealthAmountChanged(Health);
            ScoreManager.Instance.IncreaseScore(damage);
        }

        public override void LevelUpTavernTier()
        {
            if (IsMaxTavernTier() || !IsEnoughGoldForAction(LevelUpTavernTierCost))
                return;

            base.LevelUpTavernTier();
            PlayerEventManager.OnTavernTierIncreased(TavernTier);
            ScoreManager.Instance.IncreaseScore(TavernTier);
            LevelUpTavernTierCost = Field.GetOpenedCellsAmount();
        }

        public override void IncreaseGoldenCupAmount(int value)
        {
            base.IncreaseGoldenCupAmount(value);
            PlayerEventManager.SendGoldenCupAmountIncreased(GoldenCup);
            ScoreManager.Instance.IncreaseScore(value);
        }

        private void PlayDragSound(BaseUnit unit, Vector3 worldPosition)
        {
            if (Storage.IsTileOnPosition(worldPosition) || Field.IsTileOnPosition(worldPosition))
                AudioManager.Instance.PlayUnitDragSuccessfulSound();
            else
                AudioManager.Instance.PlayUnitDragFailedSound();
        }

        public override void LoadData(GameData data)
        {
            LoadDataFromMemberData(data.player);

            PlayerEventManager.SendGoldAmountChanged(Gold);
            PlayerEventManager.SendHealthAmountChanged(Health);
            PlayerEventManager.SendTavernTierIncreased(TavernTier);
            PlayerEventManager.SendGoldenCupAmountIncreased(GoldenCup);
        }

        public override void SaveData(GameData data)
        {
            SaveDataToMemberData(data.player);
        }
    }
}