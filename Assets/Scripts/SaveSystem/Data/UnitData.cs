using System;
using AutoBattler.Data.Units;

namespace AutoBattler.SaveSystem.Data
{
    [Serializable]
    public class UnitData
    {
        public string title;
        public float maxHealth;
        public float health;
        public float attackDamage;
        public float attackSpeed;

        public int x;
        public int y;

        public UnitData(BaseUnit unit, int x, int y)
        {
            title = unit.Title;
            maxHealth = unit.MaxHealth;
            health = unit.Health;
            attackDamage = unit.AttackDamage;
            attackSpeed = unit.AttackSpeed;

            this.x = x;
            this.y = y;
        }
    }
}
