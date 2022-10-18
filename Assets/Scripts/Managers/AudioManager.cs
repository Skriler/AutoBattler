using UnityEngine;
using AutoBattler.EventManagers;

namespace AutoBattler.Managers
{
    public class AudioManager : Manager<AudioManager>
    {
        [Header("UI sounds")]
        [SerializeField] private AudioSource clickSound;
        [SerializeField] private AudioSource hoverSound;
        [SerializeField] private AudioSource playButtonClickSound;

        [Header("Shop sounds")]
        [SerializeField] private AudioSource shopButtonClickSound;
        [SerializeField] private AudioSource levelUpButtonClickSound;
        [SerializeField] private AudioSource buyUnitSound;
        [SerializeField] private AudioSource unavailableActionSound;

        [Header("Drag sounds")]
        [SerializeField] private AudioSource unitDragSuccessfulSound;
        [SerializeField] private AudioSource unitDragFailedSound;

        protected override void Awake()
        {
            if (Instance != null)
                return;

            base.Awake();
            DontDestroyOnLoad(this);
        }

        public void PlayClickSound() => clickSound?.Play();

        public void PlayHoverSound() => hoverSound?.Play();

        public void PlayPlayButtonClickSound() => playButtonClickSound?.Play();

        public void PlayShopButtonClickSound() => shopButtonClickSound?.Play();

        public void PlayLevelUpButtonClickSound() => levelUpButtonClickSound?.Play();

        public void PlayBuyUnitSound() => buyUnitSound?.Play();

        public void PlayUnavailableActionSound() => unavailableActionSound?.Play();

        public void PlayUnitDragSuccessfulSound() => unitDragSuccessfulSound?.Play();

        public void PlayUnitDragFailedSound() => unitDragFailedSound?.Play();
    }
}
