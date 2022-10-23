using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AutoBattler.Managers
{
    public class AudioManager : Manager<AudioManager>
    {
        private static int MIN_VOLUME_VALUE = -60;
        private static int MAX_VOLUME_VALUE = 10;

        [Header("Mixers")]
        [SerializeField] private AudioMixer audioMixer;

        [Header("Music")]
        [SerializeField] private List<AudioClip> musicClips;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private float switchMusicWaitInterval = 1.5f;

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

            if (musicClips.Count != 0)
                PlayRandomMusicClip();
        }

        private void PlayRandomMusicClip()
        {
            int clipIndex = Random.Range(0, musicClips.Count - 1);

            if (musicClips[clipIndex] == musicSource.clip)
                _ = clipIndex == 0 ? ++clipIndex : --clipIndex;

            musicSource.clip = musicClips[clipIndex];
            musicSource.Play();

            StartCoroutine(SwitchMusicClipCoroutine());
        }

        private IEnumerator SwitchMusicClipCoroutine()
        {
            while (musicSource.isPlaying)
                yield return switchMusicWaitInterval;

            PlayRandomMusicClip();
        }

        public void SetVolume(string groupName, float volume)
        {
            audioMixer.SetFloat(groupName, Mathf.Lerp(MIN_VOLUME_VALUE, MAX_VOLUME_VALUE, volume / 100));
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
