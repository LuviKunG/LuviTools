using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace LuviKunG.UI
{
    [DisallowMultipleComponent]
    public class UserInterfaceSound : MonoBehaviour
    {
        protected const string DEFAULT_PREFAB_RESOURCE_PATH = "LuviKunG/UserInterfaceSound";
        protected static readonly string NAME = typeof(LuviConsole).Name;

        protected static UserInterfaceSound instance;
        public static UserInterfaceSound Instance
        {
            get
            {
                if (instance == null)
                {
                    UserInterfaceSound prefab = Resources.Load<UserInterfaceSound>(DEFAULT_PREFAB_RESOURCE_PATH);
                    if (prefab == null) throw new MissingReferenceException($"Cannot find {NAME} asset/prefab in resources path {DEFAULT_PREFAB_RESOURCE_PATH}.");
                    instance = Instantiate(prefab);
                    instance.name = prefab.name;
                }
                return instance;
            }
        }

        [SerializeField]
        protected AudioSource[] sourcesBGM = default;
        [SerializeField]
        protected AudioSource[] sourcesSFX = default;
        [SerializeField]
        protected AudioMixer mixer = default;
        [SerializeField]
        protected string mixerVolumeBGM = "VolumeBGM";
        [SerializeField]
        protected string mixerVolumeSFX = "VolumeSFX";

        protected Loop<AudioSource> poolBGM;
        protected Loop<AudioSource> poolSFX;
        protected AudioSource currentBGM;

        protected virtual void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
            {
                Debug.LogError($"There are 2 or more \'{NAME}\' on the scene. The new instance will be destroy.");
                Destroy(this);
                return;
            }
            poolBGM = new Loop<AudioSource>(sourcesBGM);
            poolSFX = new Loop<AudioSource>(sourcesSFX);
            DontDestroyOnLoad(gameObject);
        }

        public void PlayBGM(AudioClip clip)
        {
            if (currentBGM != null && currentBGM.clip == clip)
                return;
            AudioSource source = poolBGM.Next;
            source.volume = 1.0f;
            source.clip = clip;
            source.Play();
            if (currentBGM != null)
                StartCoroutine(CrossFade(currentBGM, source, 1.0f));
            currentBGM = source;
        }

        public void StopBGM()
        {
            if (currentBGM != null)
                StartCoroutine(Fade(currentBGM, 0.0f, 1.0f, () => currentBGM = null));
        }

        public void PlaySFX(AudioClip clip)
        {
            AudioSource source = poolSFX.Next;
            source.clip = clip;
            source.Play();
        }

        public void PlaySFX(AudioClip clip, Vector3 atPosition)
        {
            AudioSource source = poolSFX.Next;
            source.transform.position = atPosition;
            source.clip = clip;
            source.Play();
        }

        public void SetMusicSettings(bool isActive)
        {
            if (!mixer.SetFloat(mixerVolumeBGM, isActive ? 0.0f : -80.0f))
                Debug.LogWarning($"Your audio mixer you provided has no volume settings for BGM: {mixerVolumeBGM}");
        }

        public void SetSoundFXSettings(bool isActive)
        {
            if (!mixer.SetFloat(mixerVolumeSFX, isActive ? 0.0f : -80.0f))
                Debug.LogWarning($"Your audio mixer you provided has no volume settings for SFX: {mixerVolumeSFX}");
        }

        protected IEnumerator Fade(AudioSource source, float toVolume, float duration, Action onComplete)
        {
            float current = duration;
            float volume = source.volume;
            while (current > 0)
            {
                float t = current / duration;
                source.volume = Mathf.Lerp(toVolume, volume, t);
                yield return null;
                current -= Time.deltaTime;
            }
            onComplete?.Invoke();
            if (source.volume > 0)
                source.volume = toVolume;
            else
            {
                source.Stop();
                source.volume = 1.0f;
            }
        }

        protected IEnumerator CrossFade(AudioSource from, AudioSource to, float duration)
        {
            float current = duration;
            while (current > 0)
            {
                float t = current / duration;
                from.volume = Mathf.Lerp(0.0f, 1.0f, t);
                to.volume = Mathf.Lerp(1.0f, 0.0f, t);
                yield return null;
                current -= Time.deltaTime;
            }
            from.Stop();
            from.volume = 1.0f;
            to.volume = 1.0f;
        }
    }
}