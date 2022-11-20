using UnityEngine;
using AutoBattler.UnitsContainers.Containers.Field;
using AutoBattler.Data.Units;
using AutoBattler.EventManagers;
using AutoBattler.Data.ScriptableObjects.Characteristics;
using AutoBattler.Managers;
using AutoBattler.SaveSystem;
using AutoBattler.SaveSystem.Data;

namespace AutoBattler.Data.Player
{
    public class Member : MonoBehaviour
    {
        [SerializeField] protected MemberCharacteristics characteristics;

        public EnemyFieldContainer EnemyField { get; protected set; }
        public int Health { get; protected set; }
        public int Gold { get; protected set; }
        public int TavernTier { get; protected set; }
        public int LevelUpTavernTierCost { get; protected set; } = 4;

        protected int maxGainGoldPerRound;

        protected virtual void Awake()
        {
            SetStartPlayerCharacteristics();
        }

        protected virtual void Start()
        {
            EnemyField = transform.GetComponentInChildren<EnemyFieldContainer>();

            maxGainGoldPerRound = GameManager.Instance.MaxGainGoldPerRound;
        }

        protected void SetStartPlayerCharacteristics()
        {
            Health = characteristics.StartHealth;
            Gold = characteristics.StartGold;
            TavernTier = characteristics.StartTavernTier;
        }
    }
}
