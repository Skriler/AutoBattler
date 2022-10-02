using System.Collections.Generic;
using UnityEngine;
using AutoBattler.Managers;
using AutoBattler.Data.Enums;
using AutoBattler.Data.ScriptableObjects.Structs;

namespace AutoBattler.Managers
{
    public class ImageManager : Manager<ImageManager>
    {
        [SerializeField] private UnitRaceSprite[] unitRaceSpritesArray;
        [SerializeField] private UnitSpecificationSprite[] unitSpecificationSpritesArray;
        [SerializeField] private DamageTypeSprite[] damageTypeSpritesArray;

        private Dictionary<UnitRace, Sprite> unitRaceSprites;
        private Dictionary<UnitSpecification, Sprite> unitSpecificationSprites;
        private Dictionary<DamageType, Sprite> damageTypeSprites;

        private void Start()
        {
            InititalizeStructs();
        }

        public Sprite GetUnitRaceSprite(UnitRace race) => unitRaceSprites[race];

        public Sprite GetUnitSpecificationSprite(UnitSpecification specification) => unitSpecificationSprites[specification];

        public Sprite GetDamageTypeSprite(DamageType type) => damageTypeSprites[type];

        private void InititalizeStructs()
        {
            unitRaceSprites = new Dictionary<UnitRace, Sprite>();
           
            for (int i = 0; i < unitRaceSpritesArray.Length; ++i)
            {
                UnitRaceSprite unitRaceSprite = unitRaceSpritesArray[i];
                unitRaceSprites.Add(unitRaceSprite.unitRace, unitRaceSprite.sprite);
            }

            unitSpecificationSprites = new Dictionary<UnitSpecification, Sprite>();

            for (int i = 0; i < unitSpecificationSpritesArray.Length; ++i)
            {
                UnitSpecificationSprite unitSpecificationSprite = unitSpecificationSpritesArray[i];
                unitSpecificationSprites.Add(unitSpecificationSprite.unitSpecification, unitSpecificationSprite.sprite);
            }

            damageTypeSprites = new Dictionary<DamageType, Sprite>();

            for (int i = 0; i < damageTypeSpritesArray.Length; ++i)
            {
                DamageTypeSprite damageTypeSprite = damageTypeSpritesArray[i];
                damageTypeSprites.Add(damageTypeSprite.damageType, damageTypeSprite.sprite);
            }
        }
    }
}
