using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AutoBattler.Data.Buffs;

namespace AutoBattler.Data.ScriptableObjects.Databases
{
    [CreateAssetMenu(fileName = "Buff Database", menuName = "Custom/BuffDatabase")]
    public class BuffDatabase : ScriptableObject
    {
        [SerializeField] private List<SpecificationBuff> specificationBuffs;
        [SerializeField] private List<RaceBuff> raceBuffs;

        public List<SpecificationBuff> GetSpecificationBuffs()
        {
            return new List<SpecificationBuff>(specificationBuffs);
        }

        public List<RaceBuff> GetRaceBuffs()
        {
            return new List<RaceBuff>(raceBuffs);
        }

        public List<BaseBuff> GetAllBuffs()
        {
            List<BaseBuff> allBuffs = new List<BaseBuff>();
            allBuffs.AddRange(specificationBuffs);
            allBuffs.AddRange(raceBuffs);

            return allBuffs;
        }
    }
}
