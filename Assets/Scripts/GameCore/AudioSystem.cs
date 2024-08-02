using UnityEngine;
using UnityEngine.UI;

namespace Services
{
    public class AudioSystem : MonoBehaviour
    {
        [SerializeField] private int maxAudioCount = 3;
        private AudioSource[] sources;
        private int current;
        
        public AudioCueScriptableObject PlayerMissSound;
        public AudioCueScriptableObject PlayerRightSound;
        public AudioCueScriptableObject GameOverSound;
        public AudioCueScriptableObject ThreeObjectSound;
        public AudioCueScriptableObject TapSound;
        
        private float PrefsSFXVolume
        {
            get => float.Parse(PlayerPrefs.GetString("PREFS_SFXVolume", "1"));
            set => PlayerPrefs.SetString("PREFS_SFXVolume", value.ToString());
        }

        public void ChangeSFXVolume(float value)
        {
            PrefsSFXVolume = value;
        }
        
        public void Init()
        {
            sources = new AudioSource[maxAudioCount];
            for (int i = 0; i < sources.Length; i++)
                sources[i] = gameObject.AddComponent<AudioSource>();
        }
            
        public void Play(AudioCueScriptableObject audioCue, bool usePitch = true)
        {
            var s = sources[current];
            if (s.isPlaying)
                s.Stop();
            audioCue.AppendTo(s,PrefsSFXVolume ,usePitch);
            s.volume = PrefsSFXVolume;
            s.Play();
            
            current++;
            if (current >= maxAudioCount)
                current = 0;
        }

        public void PlayPlayerRightChoice()
        {
            Play(PlayerRightSound);
        }
        
        public void PlayPlayerMissChoice()
        {
            Play(PlayerMissSound);
        }
        
        public void PlayGameOverSound()
        {
            Play(GameOverSound);
        }
        
        public void PlayThreeObject()
        {
            Play(ThreeObjectSound);
        }
    }
}