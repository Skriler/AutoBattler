using System.Collections.Generic;
using UnityEngine;
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
            InitializeDictionaries();
        }

        public Sprite GetUnitRaceSprite(UnitRace race) => unitRaceSprites[race];

        public Sprite GetUnitSpecificationSprite(UnitSpecification specification) => unitSpecificationSprites[specification];

        public Sprite GetDamageTypeSprite(DamageType type) => damageTypeSprites[type];

        private void InitializeDictionaries()
        {
            unitRaceSprites = new Dictionary<UnitRace, Sprite>();
            unitSpecificationSprites = new Dictionary<UnitSpecification, Sprite>();
            damageTypeSprites = new Dictionary<DamageType, Sprite>();

            foreach (UnitRaceSprite sprite in unitRaceSpritesArray)
                unitRaceSprites.Add(sprite.unitRace, sprite.sprite);

            foreach (UnitSpecificationSprite sprite in unitSpecificationSpritesArray)
                unitSpecificationSprites.Add(sprite.unitSpecification, sprite.sprite);

            foreach (DamageTypeSprite sprite in damageTypeSpritesArray)
                damageTypeSprites.Add(sprite.damageType, sprite.sprite);
        }
    }
}
