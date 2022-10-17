using UnityEngine;

namespace AutoBattler.Managers
{
    public class AudioManager : Manager<AudioManager>
    {
        [Header("UI Sounds")]
        [SerializeField] private AudioSource clickSound;
        [SerializeField] private AudioSource hoverSound;

        protected override void Awake()
        {
            if (Instance != null)
                return;

            base.Awake();
            DontDestroyOnLoad(this);
        }

        public void PlayClickSound() => clickSound?.Play();
        public void PlayHoverSound() => hoverSound?.Play();
    }
}
