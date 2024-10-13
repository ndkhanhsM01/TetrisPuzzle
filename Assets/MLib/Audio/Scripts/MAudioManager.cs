using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace MLib
{
    public class MAudioManager: MSingleton<MAudioManager>
    {
        [SerializeField] private AudioConfigure configure;
        [SerializeField] private AudioMixer mixer;

        [SerializeField] private AudioSource sourceMusic;
        [SerializeField] private AudioSource sourceSFX;

        private Dictionary<MSoundType, MAudio> DictAudio;
        private const string nameMusicGroup = "Music";
        private const string nameSFXGroup = "SFX";
        protected override void Awake()
        {
            base.Awake();

            DictAudio = configure.GetDictAudio();
        }

        private void OnEnable()
        {
            DataManager.OnLoadLocalSuccess += OnLoadDataSuccess;
        }
        private void OnDisable()
        {
            DataManager.OnLoadLocalSuccess -= OnLoadDataSuccess;
        }
        private void OnLoadDataSuccess(LocalData data)
        {
            SetVolumeMusic(data.isPlayMusic ? 1 : 0);
            SetVolumeSFX(data.isPlaySoundFX ? 1 : 0);
        }

/*        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlaySFX(MSoundType.ClickButton);
            }
        }*/

        public void PlayMusic(MSoundType type)
        {
            if(DictAudio.TryGetValue(type, out MAudio audio))
            {
                sourceMusic.clip = audio.clip;
                sourceMusic.volume = audio.volume;
                sourceMusic.Play();
            }
            else
            {
                Debug.LogError($"Not found type <{type}> in dictionary");
            }
        }
        public void PlaySFX(MSoundType type)
        {
            if (DictAudio.TryGetValue(type, out MAudio audio))
            {
                sourceSFX.PlayOneShot(audio.clip, audio.volume);
            }
            else
            {
                Debug.LogError($"Not found type <{type}> in dictionary");
            }
        }

        /// <summary>
        /// Set volume of Music
        /// </summary>
        /// <param name="value">between 0 and 1</param>
        public void SetVolumeMusic(float value)
        {
            float percent = value;

            float newValue = -80f + (percent * 80f);

            mixer.SetFloat(nameMusicGroup, newValue);
        }

        /// <summary>
        /// Set volume of SFX
        /// </summary>
        /// <param name="value">between 0 and 1</param>
        public void SetVolumeSFX(float value)
        {
            float percent = value;

            float newValue = -80f + (percent * 80f);

            mixer.SetFloat(nameSFXGroup, newValue);
        }
    }
}